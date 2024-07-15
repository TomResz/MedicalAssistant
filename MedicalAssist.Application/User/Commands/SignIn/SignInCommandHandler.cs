using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Application.Security;
using MedicalAssist.Domain.Abstraction;
using MedicalAssist.Domain.Repositories;
using MedicalAssist.Domain.ValueObjects;

namespace MedicalAssist.Application.User.Commands.SignIn;

internal sealed class SignInCommandHandler
	: IRequestHandler<SignInCommand, SignInResponse>
{
	private readonly IPasswordManager _passwordManager;
	private readonly IUserRepository _userRepository;
	private readonly IAuthenticator _authenticator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IClock _clock;

    public SignInCommandHandler(
        IPasswordManager passwordManager,
        IUserRepository userRepository,
        IAuthenticator authenticator,
        IUnitOfWork unitOfWork,
        IRefreshTokenService refreshTokenService,
        IClock clock)
    {
        _passwordManager = passwordManager;
        _userRepository = userRepository;
        _authenticator = authenticator;
        _unitOfWork = unitOfWork;
        _refreshTokenService = refreshTokenService;
        _clock = clock;
    }

    public async Task<SignInResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var (email, password) = request;

        _ = new Email(email);
        _ = new Password(password);

        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken)
            ?? throw new InvalidSignInCredentialsException();

        ValidateUser(request, user);

        var jwt = _authenticator.GenerateToken(user);
        await RegenerateRefreshToken(user, cancellationToken);

        return new(user.Role.Value,
            user.FullName,
            jwt.AccessToken,
            user.RefreshTokenHolder.RefreshToken!);
    }

    private async Task RegenerateRefreshToken(Domain.Entites.User user, CancellationToken cancellationToken)
    {
        user.ChangeRefreshTokenHolder(_refreshTokenService.Generate(_clock.GetCurrentUtc()));
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private void ValidateUser(SignInCommand request, Domain.Entites.User user)
    {
        if (user.HasExternalLoginProvider)
        {
            throw new InvalidLoginProviderException();
        }

        if (!_passwordManager.IsValid(request.Password, user.Password!))
        {
            throw new InvalidSignInCredentialsException();
        }

        if (!user.IsVerified)
        {
            throw new UnverifiedUserException();
        }
    }
}
