using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions;
public sealed class EmptyStreetNameException : BadRequestException
{
	public EmptyStreetNameException() : base("Street is required field.")
	{
	}
}
