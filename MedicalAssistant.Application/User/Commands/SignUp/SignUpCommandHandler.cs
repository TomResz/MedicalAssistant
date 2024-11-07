using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.Security;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;

namespace MedicalAssistant.Application.User.Commands.SignUp;

internal sealed class SignUpCommandHandler : IRequestHandler<SignUpCommand>
{
    private readonly IPasswordManager _passwordManager;
    private readonly IUserRepository _userRepository;
    private readonly IClock _clock;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICodeVerification _codeVerification;
    private readonly IUserLanguageContext _userLanguageContext;
    
    public SignUpCommandHandler(
		IPasswordManager passwordManager,
		IUserRepository userRepository,
		IClock clock,
		IUnitOfWork unitOfWork,
		ICodeVerification codeVerification,
		IUserLanguageContext userLanguageContext)
	{
		_passwordManager = passwordManager;
		_userRepository = userRepository;
		_clock = clock;
		_unitOfWork = unitOfWork;
		_codeVerification = codeVerification;
		_userLanguageContext = userLanguageContext;
    }

	public async Task Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var language = _userLanguageContext.GetLanguage();
        var fullName = new FullName(request.FullName);
        var email = new Email(request.Email);
        var password = new Password(request.Password);
        var role = Role.User();
        bool isEmailUnique = await _userRepository.IsEmailUniqueAsync(email, cancellationToken);

        if (!isEmailUnique)
        {
            throw new EmailInUseException(email);
        }

        var securedPassword = _passwordManager.Secure(password);
        var verificationCode = _codeVerification.Generate(_clock.GetCurrentUtc());

        var user = Domain.Entities.User.Create(
            email,
            securedPassword,
            fullName,
            role,
         _clock.GetCurrentUtc(),
         verificationCode,
         language);

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}