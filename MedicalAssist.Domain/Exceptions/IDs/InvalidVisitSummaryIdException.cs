using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions.IDs;
public sealed class InvalidVisitSummaryIdException : BadRequestException
{
    public InvalidVisitSummaryIdException() : base("Invalid visit summary id.")
    {

    }
}
