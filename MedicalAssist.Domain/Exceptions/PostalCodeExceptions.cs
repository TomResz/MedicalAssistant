using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.Exceptions;
public sealed class InvalidPostalCodeException : BadRequestException
{
	public InvalidPostalCodeException() : base("Invalid postal code pattern.")
	{

	}
}
