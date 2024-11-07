using FluentAssertions;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.Tests.ObjectFactories;
using MedicalAssistant.Application.User.Commands.VerifyAccount;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace MedicalAssistant.Application.Tests.Commands.UserTests;
public class VerifyAccountCommandHandlerTests
{
	private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
	private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
	private readonly IClock _clock = Substitute.For<IClock>();

	private readonly VerifyAccountCommandHandler _handler;
	public VerifyAccountCommandHandlerTests()
	{
		_clock.GetCurrentUtc().Returns(DateTime.UtcNow);
		_handler = new(
			_userRepository,
			 _clock,
			  _unitOfWork);
	}

	[Fact]
	public async Task Handle_UnverifiedUser_VerifiesAccount()
	{
		var command = new VerifyAccountCommand("code-hash");
		_userRepository.GetByVerificationCodeAsync(command.CodeHash).Returns(UserFactory.CreateNotVerifiedUser());

		await _handler.Handle(command, default);

		_userRepository.Received(1).Update(Arg.Any<Domain.Entities.User>());
		await _unitOfWork.Received(1).SaveChangesAsync(default);
	}

	[Fact]
	public async Task Handle_UnknownCode_ThrowsUnknownVerificationCodeException()
	{
		var command = new VerifyAccountCommand("code-hash");
		_userRepository.GetByVerificationCodeAsync(command.CodeHash).ReturnsNull();

		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<UnknownVerificationCode>();
		_userRepository.DidNotReceive().Update(Arg.Any<Domain.Entities.User>());
		await _unitOfWork.DidNotReceive().SaveChangesAsync(default);
	}

	[Fact]
	public async Task Handle_VerifiedUser_ShouldNotUpdateEntity()
	{
		var command = new VerifyAccountCommand("code-hash");
		_userRepository.GetByVerificationCodeAsync(command.CodeHash).Returns(UserFactory.CreateUser());

		await _handler.Handle(command, default);
		_userRepository.DidNotReceive().Update(Arg.Any<Domain.Entities.User>());
		await _unitOfWork.DidNotReceive().SaveChangesAsync(default);
	}
}
