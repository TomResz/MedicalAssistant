using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions.IDs;
public sealed class InvalidVisitIdException : BadRequestException
{
    public InvalidVisitIdException() : base("Invalid visit Id.")
    {

    }
}
