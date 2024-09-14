namespace MedicalAssist.Domain.Exceptions;

public sealed class EmptyContentJsonException : BadRequestException
{
	public EmptyContentJsonException() : base("Content cannot be empty.")
	{
	}
}

public sealed class InvalidContentJsonFormatException : BadRequestException
{
	public InvalidContentJsonFormatException() : base("Cannot parse content to JSON format.")
	{
	}
}