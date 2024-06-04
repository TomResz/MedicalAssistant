using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Application.Security;
using MedicalAssist.Application.User.Commands.SignIn;
using MedicalAssist.Domain.Abstraction;
using MedicalAssist.Domain.Repositories;
using MedicalAssist.Domain.ValueObjects;
using System.Security.Claims;

namespace MedicalAssist.Application.User.Commands.ExternalLogIn;
internal sealed class ExternalLogInCommandHandler : IRequestHandler<ExternalLogInCommand,SignInResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticator _authenticator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IClock _clock;
    
  
    public ExternalLogInCommandHandler(IUserRepository userRepository, IAuthenticator authenticator, IUnitOfWork unitOfWork, IClock clock, IRefreshTokenService refreshTokenService)
    {
        _userRepository = userRepository;
        _authenticator = authenticator;
        _unitOfWork = unitOfWork;
        _clock = clock;
        _refreshTokenService = refreshTokenService;
    }

    public async Task<SignInResponse> Handle(ExternalLogInCommand request, CancellationToken cancellationToken)
    {
        var (email,providedKey,provider,fullName) = GetPayloads(request.Claims,request.Schema);

        Domain.Entites.User? user;

        user = await _userRepository.GetByEmailWithExternalProviderAsync(email, cancellationToken);

        if(user is null)
        {
            var refreshToken = _refreshTokenService.Generate(_clock.GetCurrentUtc());
            user = Domain.Entites.User.CreateByExternalProvider(email, fullName, Role.User().ToString(), _clock.GetCurrentUtc(), providedKey, provider,refreshToken);
            await _userRepository.AddAsync(user,cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        else
        {
            ValidateUser(user,  provider, providedKey);
            user.ChangeRefreshTokenHolder(_refreshTokenService.Generate(_clock.GetCurrentUtc()));
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        var jwt = _authenticator.GenerateToken(user);
        
        return new(user.Role.Value,
            user.FullName,
            jwt.AccessToken,
            user.RefreshTokenHolder.RefreshToken!,
            jwt.Expiration);
    }


    private void ValidateUser(Domain.Entites.User user,string provider,string providedKey)
    {
        if(user.ExternalUserProvider?.ProvidedKey != providedKey)
        {
            throw new InvalidExternalProvidedKeyException();
        }

        if(user.ExternalUserProvider?.Provider != provider)
        {
            throw new InvalidExternalProviderException();
        }
    }

    private (string email,string providedKey,string provider,string fullName) GetPayloads(ClaimsPrincipal claim,string? schema)
    {
        var providedKey = claim.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new EmptyClaimException("provided key") ;
        var email = claim.FindFirst(ClaimTypes.Email)?.Value ?? throw new EmptyClaimException("email");
        var firstName = claim.FindFirst(ClaimTypes.Name)?.Value ??  throw new EmptyClaimException("firstName");
        var lastName = claim.FindFirst(ClaimTypes.GivenName)?.Value ??  throw new EmptyClaimException("lastName");
        var provider = schema ??  throw new EmptyClaimException("schema");
        return (email, providedKey, provider, firstName != lastName ? $"{firstName} {lastName}" :  firstName);
    }
}
