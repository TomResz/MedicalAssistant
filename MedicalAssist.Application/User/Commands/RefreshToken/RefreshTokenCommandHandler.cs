using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Application.Exceptions.RefreshToken;
using MedicalAssist.Application.Security;
using MedicalAssist.Domain.Abstraction;
using MedicalAssist.Domain.Exceptions;
using MedicalAssist.Domain.Repositories;

namespace MedicalAssist.Application.User.Commands.RefreshToken;
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
        var email = _refreshTokenService.GetEmailFromExpiredToken(request.OldAccessToken)
            ?? throw new EmptyEmailException();

        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null)
        {
            throw new UserNotFoundException();
        }

        Validate(request, user);

        user.ChangeRefreshTokenHolder(_refreshTokenService.Generate(_clock.GetCurrentUtc()));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var jwt = _authenticator.GenerateToken(user);

        return new(
            jwt.AccessToken,
            user.RefreshTokenHolder.RefreshToken!);
    }

    private void Validate(RefreshTokenCommand request, Domain.Entites.User user)
    {
        if (user.RefreshTokenHolder.RefreshToken != request.RefreshToken)
        {
            throw new InvalidRefreshTokenException();
        }

        if (user.RefreshTokenHolder.RefreshTokenExpirationUtc?.Value <= _clock.GetCurrentUtc())
        {
            throw new RefreshTokenExpiredException();
        }
    }
}
