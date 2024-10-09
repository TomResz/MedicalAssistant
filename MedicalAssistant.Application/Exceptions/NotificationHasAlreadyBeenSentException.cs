using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;
public sealed class NotificationHasAlreadyBeenSentException : BadRequestException
{
    public NotificationHasAlreadyBeenSentException(Guid id) : base($"Notification with Id={id} has been already sent.")
    {
        
    }
}
