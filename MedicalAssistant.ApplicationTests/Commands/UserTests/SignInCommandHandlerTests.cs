using FluentAssertions;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.Security;
using MedicalAssistant.Application.Tests.ObjectFactories;
using MedicalAssistant.Application.User.Commands.SignIn;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;


namespace MedicalAssistant.Application.Tests.Commands.UserTests;
public class SignInCommandHandlerTests
{
    private readonly IPasswordManager _passwordManager = Substitute.For<IPasswordManager>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IAuthenticator _authenticator = Substitute.For<IAuthenticator>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IRefreshTokenService _refreshTokenService = Substitute.For<IRefreshTokenService>();
    private readonly IClock _clock = Substitute.For<IClock>();
    private readonly SignInCommandHandler _handler;

    public SignInCommandHandlerTests()
    {
        _clock.GetCurrentUtc().Returns(DateTime.UtcNow);

        _handler = new(
            _passwordManager,
            _userRepository,
             _authenticator,
              _unitOfWork,
                 _refreshTokenService,
                    _clock);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsSignInResponse()
    {
        var command = new SignInCommand("tom@gmail.com", "examplepassword");
        var accessToken = "AccessToken";
        var refreshToken = "RefreshToken";
        var expirationTime = DateTime.UtcNow.AddDays(1);
        var user = UserFactory.CreateUser(command.Email, command.Password);

        _passwordManager.IsValid(command.Password, user.Password!).Returns(true);
        _userRepository.GetByEmailAsync(user.Email).Returns(user);
        _authenticator.GenerateToken(user).Returns(new Dto.JwtDto { AccessToken = accessToken });
        _refreshTokenService.Generate(Arg.Any<DateTime>())
            .Returns(new RefreshTokenHolder(refreshToken, expirationTime));

        var result = await _handler.Handle(command, default);

        result.Should().NotBeNull();
        result.AccessToken.Should().Be(accessToken);
        result.RefreshToken.Should().Be(refreshToken);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_UnknownEmail_ThrowsInvalidSignInCredentialsException()
    {
        var command = new SignInCommand("tom@gmail.com", "examplepassword");

        var user = UserFactory.CreateUser(command.Email, command.Password);

        _userRepository.GetByEmailAsync(user.Email).ReturnsNull();


        Func<Task> act = async () => await _handler.Handle(command, default);

        await act.Should().ThrowAsync<InvalidSignInCredentialsException>();
        await _unitOfWork.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task Handler_InvalidPassword_ThrowsInvalidSignInCredentialsException()
    {
        var command = new SignInCommand("tom@gmail.com", "examplepassword");

        var user = UserFactory.CreateUser(command.Email, command.Password);

        _userRepository.GetByEmailAsync(user.Email).Returns(user);
        _passwordManager.IsValid(command.Password, user.Password!).Returns(false);

        Func<Task> act = async () => await _handler.Handle(command, default);

        await act.Should().ThrowAsync<InvalidSignInCredentialsException>();
        await _unitOfWork.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_NotVerifiedUser_ThrowsUnverifiedUserException()
    {
        var command = new SignInCommand("tom@gmail.com", "examplepassword");

        var user = UserFactory.CreateNotVerifiedUser(command.Email, command.Password);

        _userRepository.GetByEmailAsync(user.Email).Returns(user);
        _passwordManager.IsValid(command.Password, user.Password!).Returns(true);

        Func<Task> act = async () => await _handler.Handle(command, default);

        await act.Should().ThrowAsync<UnverifiedUserException>();
        await _unitOfWork.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_AccountWithGivenEmailHasExternalAuthProvider_ThrowsInvalidLoginProviderException()
    {
        var command = new SignInCommand("tom@gmail.com", "examplepassword");

        var user = UserFactory.CreateWithExternalAuthProvider(command.Email);

        _userRepository.GetByEmailAsync(user.Email).Returns(user);

        Func<Task> act = async () => await _handler.Handle(command, default);

        await act.Should().ThrowAsync<InvalidLoginProviderException>();
        await _unitOfWork.DidNotReceive().SaveChangesAsync();
    }
}
