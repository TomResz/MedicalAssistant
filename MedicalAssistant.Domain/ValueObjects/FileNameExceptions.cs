using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.ValueObjects;
public sealed class EmptyFileNameException : BadRequestException
{
	public EmptyFileNameException() : base("File name cannot be empty.")
	{

	}
}