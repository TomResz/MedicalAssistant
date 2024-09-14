using FluentAssertions;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.Exceptions.RefreshToken;
using MedicalAssistant.Application.Security;
using MedicalAssistant.Application.Tests.ObjectFactories;
using MedicalAssistant.Application.User.Commands.RefreshToken;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace MedicalAssistant.Application.Tests.Commands.UserTests;
public class RefreshTokenCommandHandlerTests
{
	private readonly IClock _clock = Substitute.For<IClock>();
	private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
	private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
	private readonly IRefreshTokenService _refreshTokenService = Substitute.For<IRefreshTokenService>();
	private readonly IAuthenticator _authenticator = Substitute.For<IAuthenticator>();

	private static readonly DateTime _date = DateTime.UtcNow.AddMinutes(1);	
	private readonly RefreshTokenCommandHandler _handler;
	public RefreshTokenCommandHandlerTests()
    {
		_clock.GetCurrentUtc().Returns(_date);
        _handler = new(
			_clock,
		_unitOfWork,
	_userRepository,
	 _refreshTokenService,
	 _authenticator);
    }

	[Fact]
	public async Task Handle_InvalidAccessToken_ThrowsEmptyEmailException()
	{
		var command = new RefreshTokenCommand("refresh-token", "access-token");
		_refreshTokenService.GetEmailFromExpiredToken(command.OldAccessToken).ReturnsNull();

		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<EmptyEmailException>();
		await _unitOfWork.DidNotReceive().SaveChangesAsync();
		_authenticator.DidNotReceive().GenerateToken(Arg.Any<Domain.Entites.User>());
	}

	[Fact]
	public async Task Handle_UnknownUser_ThrowsUserNotFoundException()
	{
		const string email = "email@email.com";
		var command = new RefreshTokenCommand("refresh-token", "access-token");
		_refreshTokenService.GetEmailFromExpiredToken(command.OldAccessToken).Returns(email);
		_userRepository.GetByEmailAsync(email).ReturnsNull();

		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<UserNotFoundException>();
		await _unitOfWork.DidNotReceive().SaveChangesAsync();
		_authenticator.DidNotReceive().GenerateToken(Arg.Any<Domain.Entites.User>());
	}

	[Fact]
	public async Task Handle_InvalidOldRefreshToken_ThrowsInvalidRefreshTokenException()
	{
		const string email = "email@email.com";
		var user = UserFactory.CreateUser(email);
		user.ChangeRefreshTokenHolder(new RefreshTokenHolder("another-token", DateTime.UtcNow.AddDays(1)));
		
		var command = new RefreshTokenCommand("refresh-token", "access-token");
		_refreshTokenService.GetEmailFromExpiredToken(command.OldAccessToken).Returns(email);
		_userRepository.GetByEmailAsync(email).Returns(user);

		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<InvalidRefreshTokenException>();
		await _unitOfWork.DidNotReceive().SaveChangesAsync();
		_authenticator.DidNotReceive().GenerateToken(Arg.Any<Domain.Entites.User>());
	}

	[Fact]
	public async Task Handle_ExpiredRefreshToken_ThrowsRefreshTokenExpiredException()
	{
		const string email = "email@email.com";
		var user = UserFactory.CreateUser(email);
		user.ChangeRefreshTokenHolder(new RefreshTokenHolder("refresh-token", _date.AddDays(-1)));

		var command = new RefreshTokenCommand("refresh-token", "access-token");
		_refreshTokenService.GetEmailFromExpiredToken(command.OldAccessToken).Returns(email);
		_userRepository.GetByEmailAsync(email).Returns(user);

		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<RefreshTokenExpiredException>();
		await _unitOfWork.DidNotReceive().SaveChangesAsync();
		_authenticator.DidNotReceive().GenerateToken(Arg.Any<Domain.Entites.User>());
	}

	[Fact]
	public async Task Handle_ValidCredentials_ReturnsNewAccessTokenAndRefreshToken()
	{
		const string email = "email@email.com";
		var newRefreshTokenHolder = new RefreshTokenHolder("new-refresh-token", Date.Now.AddDays(1));
		var user = UserFactory.CreateUser(email);
		user.ChangeRefreshTokenHolder(new RefreshTokenHolder("refresh-token", _date.AddHours(1)));

		var command = new RefreshTokenCommand("refresh-token", "access-token");
		_refreshTokenService.GetEmailFromExpiredToken(command.OldAccessToken).Returns(email);
		_userRepository.GetByEmailAsync(email).Returns(user);
		_refreshTokenService.Generate(_clock.GetCurrentUtc()).Returns(newRefreshTokenHolder);
		_authenticator.GenerateToken(user)
			.Returns(new Dto.JwtDto() { AccessToken = "new-access-token", Expiration = _date.AddMinutes(15) });

		var response = await _handler.Handle(command, default);
		
		response.Should().NotBeNull();
		response.RefreshToken.Should().Be(newRefreshTokenHolder.RefreshToken!);
		response.AccessToken.Should().Be("new-access-token");
		await _unitOfWork.Received(1).SaveChangesAsync();
	}
}
