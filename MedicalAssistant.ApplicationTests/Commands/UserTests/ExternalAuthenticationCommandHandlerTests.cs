using FluentAssertions;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.Security;
using MedicalAssistant.Application.Tests.ObjectFactories;
using MedicalAssistant.Application.User.Commands.ExternalAuthentication;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace MedicalAssistant.Application.Tests.Commands.UserTests;
public class ExternalAuthenticationCommandHandlerTests
{
	private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
	private readonly IClock _clock = Substitute.For<IClock>();
	private readonly IRefreshTokenService _refreshTokenService = Substitute.For<IRefreshTokenService>();
	private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
	private readonly IAuthenticator _authenticator = Substitute.For<IAuthenticator>();
	private readonly IUserLanguageContext _userLanguageContext = Substitute.For<IUserLanguageContext>();
	private static readonly DateTime _date = DateTime.UtcNow;
	private readonly ExternalAuthenticationCommandHandler _handler;
	public ExternalAuthenticationCommandHandlerTests()
	{
		_userLanguageContext.GetLanguage().Returns(Domain.Enums.Languages.English);
		_clock.GetCurrentUtc().Returns(_date);
		_handler = new(
			_userRepository,
			 _clock,
			 _refreshTokenService,
			 _unitOfWork,
			 _authenticator,
			_userLanguageContext);
	}

	[Fact]
	public async Task Handle_InvalidApiResponse_InvalidExternalAuthenticationResponseException()
	{
		var command = new ExternalAuthenticationCommand(null, "provider");
		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<InvalidExternalAuthenticationResponseException>();

		await _userRepository.DidNotReceive().GetByEmailWithExternalProviderAsync(Arg.Any<Email>());
		_authenticator.DidNotReceive().GenerateToken(Arg.Any<Domain.Entites.User>());
	}

	[Fact]
	public async Task Handle_CorrectCredentialsUserDoesNotExists_SignUpUserAndReturnsJwt()
	{
		var user = UserFactory.CreateWithExternalAuthProvider();
		var accessToken = "access-token";
		var refreshToken = "refresh-token";
		var command = new ExternalAuthenticationCommand(
			 new Dto.ExternalApiResponse(
				   user.ExternalUserProvider!.ProvidedKey,
					  user.Email,
						user.FullName),
			  user.ExternalUserProvider.Provider);

		_userRepository.GetByEmailWithExternalProviderAsync(user.Email).ReturnsNull();
		
		_refreshTokenService.Generate(_clock.GetCurrentUtc(), Arg.Any<UserId>())
			.Returns(TokenHolder.Create(refreshToken, _date.AddDays(1), user.Id));
		
		_authenticator.GenerateToken(Arg.Any<Domain.Entites.User>())
			.Returns(new Dto.JwtDto() { AccessToken = accessToken, Expiration = _date.AddMinutes(15) });

		var response = await _handler.Handle(command, default);

		response.Should().NotBeNull();
		response.AccessToken.Should().Be(accessToken);
		response.RefreshToken.Should().Be(refreshToken);

		await _unitOfWork.Received(1).SaveChangesAsync();
		await _userRepository.Received(1).AddAsync(Arg.Any<Domain.Entites.User>());
	}

	[Fact]
	public async Task Handle_CorrectCredentialsAndUserExists_SignsInUserAndReturnsJwt()
	{
		var user = UserFactory.CreateWithExternalAuthProvider();
		var accessToken = "access-token";
		var refreshToken = "refresh-token";
		var command = new ExternalAuthenticationCommand(
			 new Dto.ExternalApiResponse(
				   user.ExternalUserProvider!.ProvidedKey,
					  user.Email,
						user.FullName),
			  user.ExternalUserProvider.Provider);

		_userRepository.GetByEmailWithExternalProviderAsync(user.Email).Returns(user);
		_refreshTokenService.Generate(_clock.GetCurrentUtc(), user.Id)
			.Returns( TokenHolder.Create(refreshToken, _date.AddDays(1), user.Id));
		_authenticator.GenerateToken(Arg.Any<Domain.Entites.User>())
			.Returns(new Dto.JwtDto() { AccessToken = accessToken, Expiration = _date.AddMinutes(15) });

		var response = await _handler.Handle(command, default);

		response.Should().NotBeNull();
		response.AccessToken.Should().Be(accessToken);
		response.RefreshToken.Should().Be(refreshToken);

		await _unitOfWork.Received(1).SaveChangesAsync();
		await _userRepository.DidNotReceive().AddAsync(Arg.Any<Domain.Entites.User>());
	}

	[Fact]
	public async Task Handle_UserWithGivenEmailAlreadyHadAccount_ThrowsInvalidExternalProviderException()
	{
		var user = UserFactory.CreateWithExternalAuthProvider(
			"email@email.com",
			   "Facebook",
			   "123123");
		var accessToken = "access-token";
		var refreshToken = "refresh-token";
		var command = new ExternalAuthenticationCommand(
			 new Dto.ExternalApiResponse(
				   "321321",
					  user.Email,
						user.FullName),
			  "Google");

		_userRepository.GetByEmailWithExternalProviderAsync(user.Email).Returns(user);
		_refreshTokenService.Generate(_clock.GetCurrentUtc(), user.Id)
			.Returns( TokenHolder.Create(refreshToken, _date.AddDays(1),user.Id));
		_authenticator.GenerateToken(Arg.Any<Domain.Entites.User>())
			.Returns(new Dto.JwtDto() { AccessToken = accessToken, Expiration = _date.AddMinutes(15) });

		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<InvalidExternalProviderException>();

		await _unitOfWork.DidNotReceive().SaveChangesAsync();
		await _userRepository.DidNotReceive().AddAsync(Arg.Any<Domain.Entites.User>());
	}
}
