using FluentAssertions;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Application.Security;
using MedicalAssist.Application.User.Commands.PasswordChange;
using MedicalAssist.Domain.Enums;
using MedicalAssist.Domain.Repositories;
using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;
using Moq;

namespace MedicalAssist.Application.Tests.Commands;
public class ChangePasswordCommandHandlerTests
{
	private readonly Mock<IPasswordManager> _passwordManager;
	private readonly Mock<IUserRepository> _userRepository;
	private readonly Mock<IUserContext> _userSessionService;
	private readonly Mock<IUnitOfWork> _unitOfWork;

	private readonly Languages _language = Languages.English;

	private readonly ChangePasswordCommand _command = new("12345678", "12345678");
	public ChangePasswordCommandHandlerTests()
	{
		_passwordManager = new Mock<IPasswordManager>();
		_userRepository = new Mock<IUserRepository>();
		_userSessionService = new Mock<IUserContext>();
		_unitOfWork = new Mock<IUnitOfWork>();
	}

	[Fact]
	public async Task Handle_ValidCredentials_ShouldChangePassword()
	{
		// arrange

		var user = Domain.Entites.User.Create("tom@tom.com", "12345678", "Tom tom", Role.Admin(), Date.Now, "1234567", _language);

		_passwordManager.Setup(x => x.IsValid(It.IsAny<string>(), It.IsAny<string>()))
			.Returns(false);
		_userRepository.Setup(x => x.GetByIdAsync(It.IsAny<UserId>(),default))
			.ReturnsAsync(user);
		_userSessionService.Setup(x => x.GetUserId)
			.Returns(Guid.NewGuid());
		_passwordManager.Setup(x => x.Secure(user.Password))
			.Returns("Secured-password");

		var handler = new ChangePasswordCommandHandler(_passwordManager.Object, _userRepository.Object, _userSessionService.Object, _unitOfWork.Object);

		// act
		await handler.Handle(_command, default);

		// assert
		_unitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Once);
		_userRepository.Verify(x => x.Update(user), Times.Once);

	}

	[Fact]
	public async Task Handle_PasswordsNotMatches_ThrowsInvalidConfirmedPasswordException()
	{
		// arrange
		var command = _command with { ConfirmedPassword = "12345657887876" };
		var handler = new ChangePasswordCommandHandler(_passwordManager.Object, _userRepository.Object, _userSessionService.Object, _unitOfWork.Object);

		// act
		Func<Task> act = async () => await handler.Handle(command, default);

		// assert
		await act.Should().ThrowAsync<InvalidConfirmedPasswordException>();
		_unitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Never);
		_userRepository.Verify(x => x.Update(It.IsAny<Domain.Entites.User>()), Times.Never);

	}


	[Fact]
	public async Task Handle_SamePasswordAsOldPassword_ThrowsInvalidNewPasswordException()
	{
		// arrange

		var user = Domain.Entites.User.Create("tom@tom.com", "12345678", "Tom tom", Role.Admin(), Date.Now,"1234567", _language);

		_passwordManager.Setup(x => x.IsValid(It.IsAny<string>(), It.IsAny<string>()))
			.Returns(true);
		_userRepository.Setup(x => x.GetByIdAsync(It.IsAny<UserId>(), default))
			.ReturnsAsync(user);
		_userSessionService.Setup(x => x.GetUserId)
			.Returns(Guid.NewGuid());

		var handler = new ChangePasswordCommandHandler(_passwordManager.Object, _userRepository.Object, _userSessionService.Object, _unitOfWork.Object);

		// act
		Func<Task> act = async() => await handler.Handle(_command, default);

		// assert
		await act.Should().ThrowAsync<InvalidNewPasswordException>();
		_unitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Never);
		_userRepository.Verify(x => x.Update(user), Times.Never);

	}


	[Fact]
	public async Task Handle_UnknownUser_ThrowsUserNotFoundException()
	{
		// arrange
		_passwordManager.Setup(x => x.IsValid(It.IsAny<string>(), It.IsAny<string>()))
			.Returns(true);
		_userRepository.Setup(x => x.GetByIdAsync(It.IsAny<UserId>(), default))
			.ReturnsAsync(() => null);
		_userSessionService.Setup(x => x.GetUserId)
			.Returns(Guid.NewGuid());

		var handler = new ChangePasswordCommandHandler(_passwordManager.Object, _userRepository.Object, _userSessionService.Object, _unitOfWork.Object);

		// act
		Func<Task> act = async () => await handler.Handle(_command, default);

		// assert
		await act.Should().ThrowAsync<UserNotFoundException>();
		_unitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Never);
		_userRepository.Verify(x => x.Update(It.IsAny<Domain.Entites.User>()), Times.Never);

	}
}
