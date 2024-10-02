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

    public RefreshTokenCommandHandler(
        IClock clock,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IRefreshTokenService refreshTokenService,
        IAuthenticator authenticator)
    {
        _clock = clock;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _refreshTokenService = refreshTokenService;
        _authenticator = authenticator;
    }

    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
	{
		var id = _refreshTokenService.GetUserIdFromExpiredToken(request.OldAccessToken)
			?? throw new EmptyEmailException();

		Domain.Entites.User? user = await _userRepository.GetWithRefreshTokens(id, cancellationToken);

		if (user is null)
		{
			throw new UserNotFoundException();
		}

		Validate(request, user);

		var refreshTokenHolder = _refreshTokenService.Generate(_clock.GetCurrentUtc(), user.Id);


		user.RemoveRefreshToken(request.OldAccessToken);
		user.AddRefreshToken(refreshTokenHolder);

		_userRepository.Update(user);

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		var jwt = _authenticator.GenerateToken(user);

		return new(
			jwt.AccessToken,
			refreshTokenHolder.RefreshToken);
	}

	private void Validate(RefreshTokenCommand request, Domain.Entites.User user)
	{
		var refreshToken = user.RefreshTokens
			.Where(x => x.RefreshToken == request.RefreshToken)
			.First();

		if (refreshToken.RefreshTokenExpirationUtc < new Date(_clock.GetCurrentUtc()))
		{
			user.RemoveRefreshToken(refreshToken.RefreshToken);
			throw new RefreshTokenExpiredException();
		}
	}
}
