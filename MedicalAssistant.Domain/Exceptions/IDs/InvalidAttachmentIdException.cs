using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions.IDs;

public sealed class InvalidAttachmentIdException : BadRequestException
{
    public InvalidAttachmentIdException() : base("Attachment Id cannot be empty.")
    {

    }
}