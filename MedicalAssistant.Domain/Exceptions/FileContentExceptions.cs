namespace MedicalAssistant.Domain.Exceptions;


public sealed class EmptyFileContentException : BadRequestException
{
	public EmptyFileContentException() : base("File content cannot be empty.")
	{

	}
}

public sealed class ExceededFileContentMaximumSizeException : BadRequestException
{

	public ExceededFileContentMaximumSizeException(int maxSize) : base($"File maximum size is {maxSize} MB.")
	{

	}
}