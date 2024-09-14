using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions.IDs;
public sealed class InvalidVisitSummaryIdException : BadRequestException
{
    public InvalidVisitSummaryIdException() : base("Invalid visit summary id.")
    {

    }
}
