using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions;
public sealed class InvalidNoteLengthException : BadRequestException
{
	public InvalidNoteLengthException() : base("The note must be between 3 and 500 characters long.")
	{
	}
}
