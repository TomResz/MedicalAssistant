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
	private readonly IRefreshTokenRepository _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
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
	 _authenticator,
	 _refreshTokenRepository);
    }

	[Fact]
	public async Task Handle_InvalidAccessToken_ThrowsEmptyEmailException()
	{
		var command = new RefreshTokenCommand("refresh-token", "access-token");
		_refreshTokenService.GetUserIdFromExpiredToken(command.OldAccessToken).ReturnsNull();

		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<EmptyEmailException>();
		await _unitOfWork.DidNotReceive().SaveChangesAsync();
		_authenticator.DidNotReceive().GenerateToken(Arg.Any<Domain.Entities.User>());
	}

	[Fact]
	public async Task Handle_UnknownUser_ThrowsUserNotFoundException()
	{
		const string email = "email@email.com";
		var command = new RefreshTokenCommand("refresh-token", "access-token");
		_refreshTokenService.GetUserIdFromExpiredToken(command.OldAccessToken).Returns(new Domain.ValueObjects.IDs.UserId(Guid.
			NewGuid()));
		_userRepository.GetByEmailAsync(email).ReturnsNull();

		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<UserNotFoundException>();
		await _unitOfWork.DidNotReceive().SaveChangesAsync();
		_authenticator.DidNotReceive().GenerateToken(Arg.Any<Domain.Entities.User>());
	}

	[Fact]
	public async Task Handle_InvalidOldRefreshToken_ThrowsUserNotFoundException()
	{
		const string email = "email@email.com";
		var user = UserFactory.CreateUser(email);
		user.AddRefreshToken(
	TokenHolder.Create("refresh-token-another", _clock.GetCurrentUtc().AddDays(-1), user.Id));

		var command = new RefreshTokenCommand("refresh-token", "access-token");
		_refreshTokenService.GetUserIdFromExpiredToken(oldAccessToken: command.OldAccessToken).Returns(user.Id);
		_userRepository.GetByEmailAsync(email).ReturnsNull();

		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<UserNotFoundException>();
		await _unitOfWork.DidNotReceive().SaveChangesAsync();
		_authenticator.DidNotReceive().GenerateToken(Arg.Any<Domain.Entities.User>());
	}

	[Fact]
	public async Task Handle_ExpiredRefreshToken_ThrowsRefreshTokenExpiredException()
	{
		var user = UserFactory.CreateUser();
		user.AddRefreshToken(
			TokenHolder.Create("refresh-token", _clock.GetCurrentUtc().AddDays(-1), user.Id));

		var userId = user.Id;
		var command = new RefreshTokenCommand("refresh-token", "access-token");
		
		_refreshTokenService.GetUserIdFromExpiredToken(command.OldAccessToken).Returns(user.Id);
		_userRepository.GetWithRefreshTokens(userId,default).Returns(user);

		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<RefreshTokenExpiredException>();
		await _unitOfWork.DidNotReceive().SaveChangesAsync();
		_authenticator.DidNotReceive().GenerateToken(Arg.Any<Domain.Entities.User>());
	}
}
