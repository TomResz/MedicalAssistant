using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Application.Security;
using MedicalAssist.Application.User.Commands.SignIn;
using MedicalAssist.Domain.Abstraction;
using MedicalAssist.Domain.Repositories;
using MedicalAssist.Domain.ValueObjects;

namespace MedicalAssist.Application.User.Commands.ExternalAuthentication;
internal sealed class ExternalAuthenticationCommandHandler
	:IRequestHandler<ExternalAuthenticationCommand,SignInResponse>
{
	private readonly IUserRepository _userRepository;
	private readonly IClock _clock;
	private readonly IRefreshTokenService _refreshTokenService;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAuthenticator _authenticator;


	public ExternalAuthenticationCommandHandler(
		IUserRepository userRepository,
		IClock clock,
		IRefreshTokenService refreshTokenService,
		IUnitOfWork unitOfWork,
		IAuthenticator authenticator)
	{
		_userRepository = userRepository;
		_clock = clock;
		_refreshTokenService = refreshTokenService;
		_unitOfWork = unitOfWork;
		_authenticator = authenticator;
	}

	public async Task<SignInResponse> Handle(ExternalAuthenticationCommand request, CancellationToken cancellationToken)
	{
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
			user = Domain.Entites.User.CreateByExternalProvider(email, fullName, Role.User().ToString(), _clock.GetCurrentUtc(), id, request.Provider, refreshToken);
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
		return new(user.Role.Value,
	user.FullName,
	jwt.AccessToken,
	user.RefreshTokenHolder.RefreshToken!);
	}
	private static bool IsUserValid(Domain.Entites.User user, string provider, string userId) 
		=> user.ExternalUserProvider?.ProvidedKey == userId && user.ExternalUserProvider?.Provider == provider;


}
