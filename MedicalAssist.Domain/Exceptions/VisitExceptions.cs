using MedicalAssist.Domain.Exceptions.Shared;
using MedicalAssist.Domain.ValueObjects;

namespace MedicalAssist.Domain.Exceptions;
public sealed class VisitAlreadyExistsForGivenPeriodOfTimeException : BadRequestException
{
	public VisitAlreadyExistsForGivenPeriodOfTimeException(Date lowerBound, Date upperBound)
		: base($"Visit already exists for a given period of time : {lowerBound.Value.ToString("HH:mm dd/MM/yyyy")} and {upperBound.Value.ToString("HH:mm dd/MM/yyyy")} .")
	{

	}
}
public sealed class UnknownVisitException : BadRequestException
{
	public UnknownVisitException() : base("Unknown visit credentials.")
	{

	}
    public UnknownVisitException(Guid visitId) : base($"Visit with given id='{visitId}' was not found.")
    {

    }
}
