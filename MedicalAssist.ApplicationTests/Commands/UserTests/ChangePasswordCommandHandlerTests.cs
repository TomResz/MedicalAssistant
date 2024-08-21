using FluentAssertions;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Application.Security;
using MedicalAssist.Application.Tests.ObjectFactories;
using MedicalAssist.Application.User.Commands.PasswordChange;
using MedicalAssist.Domain.Repositories;
using MedicalAssist.Domain.ValueObjects.IDs;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace MedicalAssist.Application.Tests.Commands.UserTests;
public class ChangePasswordCommandHandlerTests
{
	private readonly IPasswordManager _passwordManager = Substitute.For<IPasswordManager>();
	private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
	private readonly IUserContext _userSessionService = Substitute.For<IUserContext>();
	private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly ChangePasswordCommandHandler _handler; 
	private static readonly Guid _userId = Guid.NewGuid(); 
    public ChangePasswordCommandHandlerTests()
    {
        _userSessionService.GetUserId.Returns(new UserId(_userId));
        _handler = new(
            _passwordManager,
            _userRepository,
            _userSessionService,
            _unitOfWork);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ShouldChangePassword()
    {
        var command = new ChangePasswordCommand(
            "zaq1@WSX",
			"zaq1@WSX");
        _userRepository.GetByIdAsync(_userId).Returns(UserFactory.CreateUser());
        _passwordManager.IsValid(command.NewPassword, Arg.Any<string>()).Returns(false);
        _passwordManager.Secure(Arg.Any<string>()).Returns($"new-password-{Guid.NewGuid()}");

        await _handler.Handle(command, default);

        _userRepository.Received(1).Update(Arg.Any<Domain.Entites.User>());
        await _unitOfWork.Received(1).SaveChangesAsync(default);
    }

    [Fact]
    public async Task Handle_NewPasswordIsSameAsOldPassword_ThrowsInvalidConfirmedPasswordException()
    {
		var command = new ChangePasswordCommand(
	"zaq1@WSX",
	"zaq1@WSX");
		_userRepository.GetByIdAsync(_userId).Returns(UserFactory.CreateUser());
		_passwordManager.IsValid(command.NewPassword, Arg.Any<string>()).Returns(true);

		Func<Task> act = async () => await _handler.Handle(command, default);

        await act.Should().ThrowAsync<InvalidNewPasswordException>();

        _userRepository.DidNotReceive().Update(Arg.Any<Domain.Entites.User>()); 
        await _unitOfWork.DidNotReceive().SaveChangesAsync();
	}


	[Fact]
    public async Task Handle_UnknownUser_ThrowsUserNotFoundException()
    {

		var command = new ChangePasswordCommand(
        "zaq1@WSX",
        "zaq1@WSX");
		_userRepository.GetByIdAsync(_userId).ReturnsNull();

		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<UserNotFoundException>();

		_userRepository.DidNotReceive().Update(Arg.Any<Domain.Entites.User>());
		await _unitOfWork.DidNotReceive().SaveChangesAsync();
	}

    [Fact]
    public async Task Handle_PasswordsNotMatches_ThrowsInvalidConfirmedPasswordException()
    {
		var command = new ChangePasswordCommand(
    "zaq1@WSX",
    "zaq123456@WSX");

        Func<Task> act = async () => await _handler.Handle(command, default);

        await act.Should().ThrowAsync<InvalidConfirmedPasswordException>();
		_userRepository.DidNotReceive().Update(Arg.Any<Domain.Entites.User>());
		await _unitOfWork.DidNotReceive().SaveChangesAsync();
	}
}
