using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions.IDs;
public sealed class InvalidVisitIdException : BadRequestException
{
    public InvalidVisitIdException() : base("Invalid visit Id.")
    {

    }
}
