using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.Exceptions;
public sealed class EmptyStreetNameException : BadRequestException
{
	public EmptyStreetNameException() : base("Street is required field.")
	{
	}
}
