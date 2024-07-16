using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.Exceptions.IDs;
public sealed class InvalidVisitIdException : BadRequestException
{
    public InvalidVisitIdException() : base("Invalid visit Id.")
    {

    }
}
