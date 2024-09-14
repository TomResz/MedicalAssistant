using FluentAssertions;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.Security;
using MedicalAssistant.Application.Tests.ObjectFactories;
using MedicalAssistant.Application.User.Commands.PasswordChangeByCode;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace MedicalAssistant.Application.Tests.Commands.UserTests;
public class PasswordChangeByCodeHandlerTests
{
	private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
	private readonly IEmailCodeManager _emailCodeManager = Substitute.For<IEmailCodeManager>();
	private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
	private readonly IPasswordManager _passwordManager = Substitute.For<IPasswordManager>();

	private readonly PasswordChangeByCodeHandler _handler;
    public PasswordChangeByCodeHandlerTests()
    {
		_handler = new(
			_emailCodeManager,
			  _userRepository,
				 _unitOfWork,
				   _passwordManager);
    }

	[Fact]
	public async Task Handle_InvalidCode_ThrowsInvalidCodeException()
	{
		var command = new PasswordChangeByCodeCommand("code","new-Password123!@");
		_emailCodeManager.Decode(command.Code, out var email).Returns(false);

		Func<Task> act = async () => await _handler.Handle(command, default);
		
		await act.Should().ThrowAsync<InvalidCodeException>();
		await _userRepository.DidNotReceive().GetByEmailAsync(Arg.Any<Email>());
		_passwordManager.DidNotReceive().Secure(Arg.Any<string>());
		await _unitOfWork.DidNotReceive().SaveChangesAsync();
	}

	[Fact]
	public async Task Handle_UnknownUser_ThrowsUserNotFoundException()
	{
		var command = new PasswordChangeByCodeCommand("code", "new-Password123!@");
		_emailCodeManager.Decode(command.Code, out Arg.Any<string>()).Returns(x=> 
		{
			x[1] = "email@email.com";
			return true;
		});
		_userRepository.GetByEmailAsync(Arg.Any<Email>()).ReturnsNull();

		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<UserNotFoundException>();
		_passwordManager.DidNotReceive().Secure(Arg.Any<string>());
		await _unitOfWork.DidNotReceive().SaveChangesAsync();
	}


	[Fact]
	public async Task Handle_ValidCodeAndKnownUser_ChangesPassword()
	{
		var command = new PasswordChangeByCodeCommand("code", "new-Password123!@");
		_emailCodeManager.Decode(command.Code, out Arg.Any<string>()).Returns(x =>
		{
			x[1] = "email@email.com";
			return true;
		});
		_userRepository.GetByEmailAsync(Arg.Any<Email>()).Returns(
			UserFactory.CreateUser("email@email.com"));
		_passwordManager.Secure(command.NewPassword).Returns("hashed-password");
		await _handler.Handle(command, default);

		_passwordManager.Received(1).Secure(command.NewPassword);
		await _unitOfWork.Received(1).SaveChangesAsync();
	}
}
