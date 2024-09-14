using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions;
public sealed class EmptyPasswordException : BadRequestException
{
	public EmptyPasswordException() : base("The password provided is empty.")
	{
	}
}
public sealed class InvalidPasswordLengthException : BadRequestException
{
	public InvalidPasswordLengthException() : base("The password must be between 8 and 200 characters long.")
	{

	}
}
