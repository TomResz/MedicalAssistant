using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Application.Security;
using MedicalAssist.Domain.Repositories;

namespace MedicalAssist.Application.User.Commands.PasswordChangeByEmail;
internal sealed class PasswordChangeByEmailCommandHandler : IRequestHandler<PasswordChangeByEmailCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailCodeManager _emailCodeManager;
    private readonly IUnitOfWork _unitOfWork;
    public PasswordChangeByEmailCommandHandler(IUserRepository userRepository, IEmailCodeManager emailCodeManager, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _emailCodeManager = emailCodeManager;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(PasswordChangeByEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email,cancellationToken);
        
        if (user is null)
        {
            throw new UserNotFoundException(request.Email);
        }

        var code = _emailCodeManager.Encode(user.Email);
        user.SendEmailForPasswordChange(code);
        await _unitOfWork.SaveChangesAsync(cancellationToken);   
    }
}
