using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions;
public sealed class InvalidPostalCodeException : BadRequestException
{
	public InvalidPostalCodeException() : base("Invalid postal code pattern.")
	{

	}
}
