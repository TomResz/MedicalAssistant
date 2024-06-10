using FluentAssertions;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Application.Security;
using MedicalAssist.Application.User.Commands.SignIn;
using MedicalAssist.Domain.Abstraction;
using MedicalAssist.Domain.ComplexTypes;
using MedicalAssist.Domain.Exceptions;
using MedicalAssist.Domain.Repositories;
using MedicalAssist.Domain.ValueObjects;
using Moq;
using System.Reflection;

namespace MedicalAssist.Application.Tests.Commands;
public class SignInCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IAuthenticator> _authenticatorMock;
    private readonly Mock<IPasswordManager> _passwordManagerMock;
    private readonly Mock<IClock> _clockMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IRefreshTokenService> _refreshTokenServiceMock;

    private readonly RefreshTokenHolder _refreshTokenHolder = new("hash", DateTime.UtcNow);

    private readonly static DateTime _time = DateTime.UtcNow;   


    public SignInCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _authenticatorMock = new Mock<IAuthenticator>();
        _passwordManagerMock = new Mock<IPasswordManager>();
        _clockMock = new Mock<IClock>();    
        _unitOfWorkMock = new Mock<IUnitOfWork>();  
        _refreshTokenServiceMock = new Mock<IRefreshTokenService>();

        _clockMock.Setup(x => x.GetCurrentUtc())
            .Returns(_time);

        _refreshTokenServiceMock.Setup(x => x.Generate(_clockMock.Object.GetCurrentUtc()))
            .Returns(() => new("Hash", DateTime.UtcNow));
    }

    [Fact]
    public async Task Handle_CorrectValues_ReturnsSignInResponse()
    {

		// arrange
		var command = new SignInCommand("tom@tom.com", "12345678");

		var fullName = "Tom T";
        var role = Role.Admin();
        string? accessToken = "ExampleToken";
        var user = Domain.Entites.User.Create(
            command.Email, command.Password, fullName, role, DateTime.UtcNow, "1234s");
		var isVerifiedField = user.GetType().GetField("<IsVerified>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
		isVerifiedField?.SetValue(user, true); 

		_userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<Email>(), default))
            .ReturnsAsync(user);

        _passwordManagerMock.Setup(x=> x.IsValid(It.IsAny<string>(),It.IsAny<string>()))
            .Returns(true);
        
        _authenticatorMock.Setup(x => x.GenerateToken(It.IsAny<Domain.Entites.User>()))
            .Returns(() => new() { AccessToken =  accessToken});

        var commandHandler = new SignInCommandHandler(_passwordManagerMock.Object,
            _userRepositoryMock.Object,
            _authenticatorMock.Object,
            _unitOfWorkMock.Object,
            _refreshTokenServiceMock.Object,
            _clockMock.Object);
        _passwordManagerMock.Setup(x => x.IsValid(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);
        // act
        var response = await commandHandler.Handle(command, default);
        
        // assert
        
        response.Should().NotBeNull();
        response.GetType().Should().Be(typeof(SignInResponse));
        response.Token.Should().Be(accessToken);
        response.Role.Should().Be(role);
        response.FullName.Should().Be(fullName);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_UnknownEmail_ThrowsInvalidSignInCredentialsException()
    {

        // arrange
        var command = new SignInCommand("tom@tom.com", "12345678");

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<Email>(),default))
            .ReturnsAsync(() => null);



        var commandHandler = new SignInCommandHandler(_passwordManagerMock.Object,
            _userRepositoryMock.Object,
            _authenticatorMock.Object,
            _unitOfWorkMock.Object,
            _refreshTokenServiceMock.Object,
            _clockMock.Object);
        // act
        Func<Task> act = () =>  commandHandler.Handle(command, default);

        // assert
        await act.Should().ThrowAsync<InvalidSignInCredentialsException>();
    }

    [Fact]
    public async Task Handle_UnknownPassword_ThrowsInvalidSignInCredentialsException()
    {

        // arrange
        var fullName = "Tom T";
        var role = Role.Admin();

        var command = new SignInCommand("tom@tom.com", "12345678");

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email, default))
            .ReturnsAsync(() =>
            Domain.Entites.User.Create(command.Email, command.Password, fullName, role, DateTime.UtcNow,"123345"));

        _passwordManagerMock.Setup(x => x.IsValid(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);


        var commandHandler = new SignInCommandHandler(_passwordManagerMock.Object,
            _userRepositoryMock.Object,
            _authenticatorMock.Object,
            _unitOfWorkMock.Object,
            _refreshTokenServiceMock.Object,
            _clockMock.Object);

        // act
        Func<Task> act = () => commandHandler.Handle(command, default);

        // assert
        await act.Should().ThrowAsync<InvalidSignInCredentialsException>();
    }


    [Theory]
    [InlineData("tomt")]
    [InlineData("delta@delat")]
    [InlineData("delta@delat.")]
    [InlineData("delta.delat.")]
    public async Task Handle_InvalidEmailPattern_ThrowsIncorrectEmailPatternException(string email)
    {

        // arrange
        var command = new SignInCommand(email, "12345678");


        var commandHandler = new SignInCommandHandler(_passwordManagerMock.Object,
            _userRepositoryMock.Object,
            _authenticatorMock.Object,
            _unitOfWorkMock.Object,
            _refreshTokenServiceMock.Object,
            _clockMock.Object);
        // act
        Func<Task> act = () =>  commandHandler.Handle(command, default);

        // assert
        await act.Should().ThrowAsync<IncorrectEmailPatternException>();
    }

    [Fact]
    public async Task Handle_EmptyEmail_ThrowsEmptyEmailException()
    {

        // arrange
        var command = new SignInCommand(string.Empty, "12345678");


        var commandHandler = new SignInCommandHandler(_passwordManagerMock.Object,
            _userRepositoryMock.Object,
            _authenticatorMock.Object,
            _unitOfWorkMock.Object,
            _refreshTokenServiceMock.Object,
            _clockMock.Object);
        // act
        Func<Task> act = () => commandHandler.Handle(command, default);

        // assert
        await act.Should().ThrowAsync<EmptyEmailException>();
    }

    [Theory]
    [InlineData("tomt")]
    [InlineData("1234567")]
    public async Task Handle_InvalidPasswordLength_ThrowsInvalidPasswordLengthException(string password)
    {

        // arrange
        var command = new SignInCommand("tomt@gmail.com", password);


        var commandHandler = new SignInCommandHandler(_passwordManagerMock.Object,
            _userRepositoryMock.Object,
            _authenticatorMock.Object,
            _unitOfWorkMock.Object,
            _refreshTokenServiceMock.Object,
            _clockMock.Object);
        // act
        Func<Task> act = () => commandHandler.Handle(command, default);

        // assert
        await act.Should().ThrowAsync<InvalidPasswordLengthException>();
    }

    [Fact]
    public async Task Handle_EmptyPassword_ThrowsEmptyPasswordException()
    {

        // arrange
        var command = new SignInCommand("tomt@gmail.com", string.Empty);


        var commandHandler = new SignInCommandHandler(_passwordManagerMock.Object,
            _userRepositoryMock.Object,
            _authenticatorMock.Object,
            _unitOfWorkMock.Object,
            _refreshTokenServiceMock.Object,
            _clockMock.Object);
        // act
        Func<Task> act = () => commandHandler.Handle(command, default);

        // assert
        await act.Should().ThrowAsync<EmptyPasswordException>();
    }
}
