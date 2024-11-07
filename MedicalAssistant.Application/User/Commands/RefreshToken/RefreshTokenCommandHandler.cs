using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.Exceptions.RefreshToken;
using MedicalAssistant.Application.Security;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;

namespace MedicalAssistant.Application.User.Commands.RefreshToken;
internal sealed class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly IClock _clock;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IAuthenticator _authenticator;
	private readonly IRefreshTokenRepository _refreshTokenRepository;
	public RefreshTokenCommandHandler(
		IClock clock,
		IUnitOfWork unitOfWork,
		IUserRepository userRepository,
		IRefreshTokenService refreshTokenService,
		IAuthenticator authenticator,
		IRefreshTokenRepository refreshTokenRepository)
	{
		_clock = clock;
		_unitOfWork = unitOfWork;
		_userRepository = userRepository;
		_refreshTokenService = refreshTokenService;
		_authenticator = authenticator;
		_refreshTokenRepository = refreshTokenRepository;
	}

	public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
	{
		var id = _refreshTokenService.GetUserIdFromExpiredToken(request.OldAccessToken)
			?? throw new EmptyEmailException();

		Domain.Entities.User? user = await _userRepository.GetWithRefreshTokens(id, cancellationToken);

		if (user is null)
		{
			throw new UserNotFoundException();
		}

		await Validate(request, user,cancellationToken);

		var refreshTokenHolder = _refreshTokenService.Generate(_clock.GetCurrentUtc(), user.Id);


		user.RemoveRefreshToken(request.OldAccessToken);
		user.AddRefreshToken(refreshTokenHolder);

		_userRepository.Update(user);

		await _refreshTokenRepository.DeleteAsync(request.OldAccessToken,cancellationToken);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		var jwt = _authenticator.GenerateToken(user);

		return new(
			jwt.AccessToken,
			refreshTokenHolder.RefreshToken);
	}

	private async Task Validate(RefreshTokenCommand request, Domain.Entities.User user,CancellationToken ct)
	{
		var refreshToken = user.RefreshTokens
			.Where(x => x.RefreshToken == request.RefreshToken)
			.First();

		if (refreshToken.RefreshTokenExpirationUtc < new Date(_clock.GetCurrentUtc()))
		{
			user.RemoveRefreshToken(refreshToken.RefreshToken);
			await _refreshTokenRepository.DeleteAsync(refreshToken.RefreshToken,ct);
			throw new RefreshTokenExpiredException();
		}
	}
}
