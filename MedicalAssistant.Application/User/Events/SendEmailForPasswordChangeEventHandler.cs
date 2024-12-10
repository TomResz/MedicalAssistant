using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Events;
using MedicalAssistant.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MedicalAssistant.Application.User.Events;
public sealed class SendEmailForPasswordChangeEventHandler : INotificationHandler<SendEmailForPasswordChangeEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<SendEmailForPasswordChangeEventHandler> _logger;
    public SendEmailForPasswordChangeEventHandler(
        IUserRepository userRepository,
        IEmailService emailService,
        ILogger<SendEmailForPasswordChangeEventHandler> logger)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Handle(SendEmailForPasswordChangeEvent notification, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(notification.UserId, cancellationToken);

        if (user is null)
        {
            throw new UserNotFoundException();
        }

        await _emailService.SendMailWithChangePasswordCode(user.Email, notification.Code,notification.Language);
        _logger.LogInformation(
            "Password change email sent to {Email} for UserId {UserId} with Code {Code} and Language {Language}.",
            user.Email.Value,
            notification.UserId,
            notification.Code,
            notification.Language.ToString()
        );
    }
}
