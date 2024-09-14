using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions;
public sealed class InvalidPostalCodeException : BadRequestException
{
	public InvalidPostalCodeException() : base("Invalid postal code pattern.")
	{

	}
}
