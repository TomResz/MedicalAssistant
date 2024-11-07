using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.Security;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;

namespace MedicalAssistant.Application.User.Commands.SignIn;

internal sealed class SignInCommandHandler
	: IRequestHandler<SignInCommand, SignInResponse>
{
	private readonly IPasswordManager _passwordManager;
	private readonly IUserRepository _userRepository;
	private readonly IAuthenticator _authenticator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IClock _clock;
    private readonly ITokenRepository _tokenRepository;
	public SignInCommandHandler(
		IPasswordManager passwordManager,
		IUserRepository userRepository,
		IAuthenticator authenticator,
		IUnitOfWork unitOfWork,
		IRefreshTokenService refreshTokenService,
		IClock clock,
		ITokenRepository tokenRepository)
	{
		_passwordManager = passwordManager;
		_userRepository = userRepository;
		_authenticator = authenticator;
		_unitOfWork = unitOfWork;
		_refreshTokenService = refreshTokenService;
		_clock = clock;
		_tokenRepository = tokenRepository;
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

		var token = _refreshTokenService.Generate(_clock.GetCurrentUtc(), user.Id);

		user.AddRefreshToken(token);
        _tokenRepository.Add(token);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return new(
            jwt.AccessToken,
            token.RefreshToken);
    }

    private void ValidateUser(SignInCommand request, Domain.Entities.User user)
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
