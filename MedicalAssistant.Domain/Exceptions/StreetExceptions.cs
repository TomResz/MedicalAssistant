using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions;
public sealed class EmptyStreetNameException : BadRequestException
{
	public EmptyStreetNameException() : base("Street is required field.")
	{
	}
}
