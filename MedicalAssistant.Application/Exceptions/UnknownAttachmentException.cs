using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;
public sealed class UnknownAttachmentException : BadRequestException
{
    public UnknownAttachmentException(Guid id) : base($"Attachment with Id='{id}' was not found.")
    {
        
    }
}
