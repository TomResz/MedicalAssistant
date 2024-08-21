using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Application.Security;
using MedicalAssist.Domain.Repositories;
using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Application.User.Commands.PasswordChange;

internal sealed class ChangePasswordCommandHandler
    : IRequestHandler<ChangePasswordCommand>
{
    private readonly IPasswordManager _passwordManager;
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userSessionService;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordCommandHandler(
        IPasswordManager passwordManager,
        IUserRepository userRepository,
        IUserContext userSessionService,
        IUnitOfWork unitOfWork)
    {
        _passwordManager = passwordManager;
        _userRepository = userRepository;
        _userSessionService = userSessionService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var newPassword = new Password(request.NewPassword);
        var confirmedPassword = new Password(request.ConfirmedPassword);

        if (!newPassword.Equals(confirmedPassword))
        {
            throw new InvalidConfirmedPasswordException();
        }


        UserId userId = _userSessionService.GetUserId;

        Domain.Entites.User? user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            throw new UserNotFoundException();
        }

        bool isNewPasswordInvalid = _passwordManager.IsValid(request.NewPassword, user.Password);

		if (isNewPasswordInvalid)
        {
            throw new InvalidNewPasswordException();
        }

        var password = _passwordManager.Secure(newPassword);
        user.ChangePassword(password);

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}