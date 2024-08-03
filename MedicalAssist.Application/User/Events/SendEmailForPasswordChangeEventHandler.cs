using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Domain.Events;
using MedicalAssist.Domain.Repositories;

namespace MedicalAssist.Application.User.Events;
internal sealed class SendEmailForPasswordChangeEventHandler : INotificationHandler<SendEmailForPasswordChangeEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    public SendEmailForPasswordChangeEventHandler(IUserRepository userRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task Handle(SendEmailForPasswordChangeEvent notification, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(notification.UserId, cancellationToken);

        if (user is null)
        {
            throw new UserNotFoundException();
        }

        await _emailService.SendMailWithChangePasswordCode(user.Email, notification.Code,notification.Language);
    }
}
