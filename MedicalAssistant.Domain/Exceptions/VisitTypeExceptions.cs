using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions;
public sealed class EmptyVisitTypePropertyException : BadRequestException
{
	public EmptyVisitTypePropertyException() : base("Visit type cannot be empty.")
	{

	}
}

public sealed class VisitTypeLengthExceededException : BadRequestException
{
	public VisitTypeLengthExceededException(int maxLength) : base($"The maximum visit type length ({maxLength} characters) was exceeded.")

	{

	}
}

