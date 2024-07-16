using FluentAssertions;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Application.Security;
using MedicalAssist.Application.User.Commands.SignUp;
using MedicalAssist.Domain.Abstraction;
using MedicalAssist.Domain.Repositories;
using Moq;

namespace MedicalAssist.Application.Tests.Commands;
public class SignUpCommandHandlerTests
{
    private readonly Mock<IPasswordManager> _passwordManager;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IClock> _clock;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<ICodeVerification> _codeVerification;

    private readonly SignUpCommand _command = new("Tom Tom", "tom@tom.com", "12345678");

    private readonly static DateTime _time = DateTime.UtcNow;

    public SignUpCommandHandlerTests()
    {
        _passwordManager = new Mock<IPasswordManager>();
        _userRepository = new Mock<IUserRepository>();
        _clock = new Mock<IClock>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _codeVerification = new Mock<ICodeVerification>();
        
        _clock.Setup(x => x.GetCurrentUtc())
            .Returns(() => _time);
        
        _codeVerification.Setup(x => x.Generate(_clock.Object.GetCurrentUtc()))
            .Returns("ExampleCODEhash");
    }

    [Fact]
    public async Task Handle_ValidCredentials_CreatesNewAccount()
    {
        // arrange
        _userRepository.Setup(x => x.IsEmailUniqueAsync(_command.Email, default))
            .ReturnsAsync(true);
        _passwordManager.Setup(x => x.Secure(_command.Password))
            .Returns("hashed-password");

        var handler = new SignUpCommandHandler(_passwordManager.Object,
            _userRepository.Object, _clock.Object, _unitOfWork.Object, _codeVerification.Object);

        // act
        await handler.Handle(_command, default);

        // assert
        _userRepository.Verify(x => x.AddAsync(It.IsAny<Domain.Entites.User>(), CancellationToken.None), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_NotUniqueEmail_ThrowsEmailInUseException()
    {
        // arrange
        _userRepository.Setup(x => x.IsEmailUniqueAsync(_command.Email, default))
            .ReturnsAsync(false);
        _passwordManager.Setup(x => x.Secure(_command.Password))
            .Returns("hashed-password");

        _clock.Setup(x => x.GetCurrentUtc())
            .Returns(() => DateTime.UtcNow);

        var handler = new SignUpCommandHandler(_passwordManager.Object,
            _userRepository.Object, _clock.Object, _unitOfWork.Object, _codeVerification.Object);

        // act
        Func<Task> act = () => handler.Handle(_command, default);

        // assert
        await act.Should().ThrowAsync<EmailInUseException>();
        _userRepository.Verify(x => x.AddAsync(It.IsAny<Domain.Entites.User>(), default), Times.Never);
        _unitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
}
