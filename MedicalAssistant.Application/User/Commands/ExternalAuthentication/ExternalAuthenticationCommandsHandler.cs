﻿using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.Security;
using MedicalAssistant.Application.User.Commands.SignIn;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;

namespace MedicalAssistant.Application.User.Commands.ExternalAuthentication;
internal sealed class ExternalAuthenticationCommandHandler
	:IRequestHandler<ExternalAuthenticationCommand,SignInResponse>
{
	private readonly IUserRepository _userRepository;
	private readonly IClock _clock;
	private readonly IRefreshTokenService _refreshTokenService;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAuthenticator _authenticator;
	private readonly IUserLanguageContext _userLanguageContext;

	public ExternalAuthenticationCommandHandler(
		IUserRepository userRepository,
		IClock clock,
		IRefreshTokenService refreshTokenService,
		IUnitOfWork unitOfWork,
		IAuthenticator authenticator,
		IUserLanguageContext userLanguageContext)
	{
		_userRepository = userRepository;
		_clock = clock;
		_refreshTokenService = refreshTokenService;
		_unitOfWork = unitOfWork;
		_authenticator = authenticator;
		_userLanguageContext = userLanguageContext;
	}

	public async Task<SignInResponse> Handle(ExternalAuthenticationCommand request, CancellationToken cancellationToken)
	{
		var language =  _userLanguageContext.GetLanguage();
		var response = request.response;
		if (response is null)
		{
			throw new InvalidExternalAuthenticationResponseException();
		}

		var (id, email, fullName) = response;
		Domain.Entites.User? user = await _userRepository.GetByEmailWithExternalProviderAsync(response.Email, cancellationToken);

		if (user is null)
		{
			var refreshToken = _refreshTokenService.Generate(_clock.GetCurrentUtc());
			user = Domain.Entites.User.CreateByExternalProvider(email, fullName, Role.User().ToString(), _clock.GetCurrentUtc(), id, request.Provider, refreshToken,language);
			await _userRepository.AddAsync(user, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
		}
		else
		{
			if (!IsUserValid(user, request.Provider, response.Id))
			{
				throw new InvalidExternalProviderException("User with given email already exists!");
			}

			user.ChangeRefreshTokenHolder(_refreshTokenService.Generate(_clock.GetCurrentUtc()));
			await _unitOfWork.SaveChangesAsync(cancellationToken);
		}

		var jwt = _authenticator.GenerateToken(user);
		return new(
	jwt.AccessToken,
	user.RefreshTokenHolder.RefreshToken!);
	}
	private static bool IsUserValid(Domain.Entites.User user, string provider, string userId) 
		=> user.ExternalUserProvider?.ProvidedKey == userId && user.ExternalUserProvider?.Provider == provider;


}