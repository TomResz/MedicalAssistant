using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;
public sealed class UnknownVisitNotificationException : BadRequestException
{
    public UnknownVisitNotificationException(Guid id) : base($"Visit notification with Id='{id}' does not exists.")
    {
        
    }
}
