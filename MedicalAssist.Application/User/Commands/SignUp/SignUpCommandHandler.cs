using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Application.Security;
using MedicalAssist.Domain.Abstraction;
using MedicalAssist.Domain.Repositories;
using MedicalAssist.Domain.ValueObjects;

namespace MedicalAssist.Application.User.Commands.SignUp;

internal sealed class SignUpCommandHandler : IRequestHandler<SignUpCommand>
{
    private readonly IPasswordManager _passwordManager;
    private readonly IUserRepository _userRepository;
    private readonly IClock _clock;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICodeVerification _codeVerification;

	public SignUpCommandHandler(
		IPasswordManager passwordManager,
		IUserRepository userRepository,
		IClock clock,
		IUnitOfWork unitOfWork,
		ICodeVerification codeVerification)
	{
		_passwordManager = passwordManager;
		_userRepository = userRepository;
		_clock = clock;
		_unitOfWork = unitOfWork;
		_codeVerification = codeVerification;
	}

	public async Task Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var fullName = new FullName(request.FullName);
        var email = new Email(request.Email);
        var password = new Password(request.Password);
        var role = new Role(request.Role);

        bool isEmailUnique = await _userRepository.IsEmailUniqueAsync(email, cancellationToken);

        if (!isEmailUnique)
        {
            throw new EmailInUseException(email);
        }

        var securedPassword = _passwordManager.Secure(password);
        var verificationCode = _codeVerification.Generate(_clock.GetCurrentUtc());

        var user = Domain.Entites.User.Create(
            email,
            securedPassword,
            fullName,
            role,
         _clock.GetCurrentUtc(),
         verificationCode);

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}