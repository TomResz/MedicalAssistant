using FluentAssertions;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.Security;
using MedicalAssistant.Application.Tests.ObjectFactories;
using MedicalAssistant.Application.User.Commands.PasswordChangeByEmail;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace MedicalAssistant.Application.Tests.Commands.UserTests;
public class PasswordChangeByEmailCommandHandlerTests
{
	private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
	private readonly IEmailCodeManager _emailCodeManager = Substitute.For<IEmailCodeManager>();
	private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
	private readonly IUserLanguageContext _userLanguageContext = Substitute.For<IUserLanguageContext>();

	private readonly PasswordChangeByEmailCommandHandler _handler;

	public PasswordChangeByEmailCommandHandlerTests()
	{
		_userLanguageContext.GetLanguage().Returns(Domain.Enums.Languages.Polish);
		_handler = new(
			_userRepository,
			 _emailCodeManager,
			 _unitOfWork,
			  _userLanguageContext);
	}

	[Fact]
	public async Task Handle_ValidEmail_SendsEmail()
	{
		var command = new PasswordChangeByEmailCommand("email@email.com");
		_userRepository.GetByEmailAsync(command.Email).Returns(UserFactory.CreateUser(command.Email));
		_emailCodeManager.Encode(command.Email).Returns("code");

		await _handler.Handle(command, default);

		await _unitOfWork.Received(1).SaveChangesAsync(default);
	}

	[Fact]
	public async Task Handle_AccountHasExternalAuthProvider_ThrowsUserWithExternalProviderCannotChangePasswordException()
	{
		var command = new PasswordChangeByEmailCommand("email@email.com");
		_userRepository.GetByEmailAsync(command.Email).Returns(UserFactory.CreateWithExternalAuthProvider(command.Email));

		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<UserWithExternalProviderCannotChangePasswordException>();
		await _unitOfWork.DidNotReceive().SaveChangesAsync(default);

	}


	[Fact]
	public async Task Handle_UnknownEmail_ThrowsUserNotFoundException()
	{
		var command = new PasswordChangeByEmailCommand("email@email.com");
		_userRepository.GetByEmailAsync(command.Email).ReturnsNull();

		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<UserNotFoundException>();
		await _unitOfWork.DidNotReceive().SaveChangesAsync(default);

	}
}
