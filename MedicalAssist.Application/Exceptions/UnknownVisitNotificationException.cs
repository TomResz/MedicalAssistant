using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Application.Exceptions;
public sealed class UnknownVisitNotificationException : BadRequestException
{
    public UnknownVisitNotificationException(Guid id) : base($"Visit notification with Id='{id}' does not exists.")
    {
        
    }
}
