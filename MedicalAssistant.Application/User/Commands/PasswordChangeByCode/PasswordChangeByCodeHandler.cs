using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.Security;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;

namespace MedicalAssistant.Application.User.Commands.PasswordChangeByCode;
internal sealed class PasswordChangeByCodeHandler : ICommandHandler<PasswordChangeByCodeCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailCodeManager _emailCodeManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordManager _passwordManager;
    public PasswordChangeByCodeHandler(
		IEmailCodeManager emailCodeManager,
		IUserRepository userRepository,
		IUnitOfWork unitOfWork,
		IPasswordManager passwordManager)
    {
        _emailCodeManager = emailCodeManager;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordManager = passwordManager;
    }

    public async Task Handle(PasswordChangeByCodeCommand request, CancellationToken cancellationToken)
    {
        var isCodeCorrect = _emailCodeManager.Decode(request.Code,out string email);

        if(!isCodeCorrect)
        {
            throw new InvalidCodeException(request.Code);
        }
        var password = new Password(request.NewPassword);

        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        if(user is null)
        {
            throw new UserNotFoundException();
        }
        var securedPassword = _passwordManager.Secure(password);
        user.ChangePassword(securedPassword);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
