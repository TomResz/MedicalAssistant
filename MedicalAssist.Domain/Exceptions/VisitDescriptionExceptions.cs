using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.Exceptions;
public sealed class EmptyVisitDescriptionPropertyException
	: BadRequestException
{
	public EmptyVisitDescriptionPropertyException() : base("Visit description cannot be empty.")
	{

	}
}

public sealed class VisitDescriptionLengthExceededException : BadRequestException
{
	public VisitDescriptionLengthExceededException(int maxLength) : base($"The maximum visit description length ({maxLength} characters) was exceeded.")
	{

	}
}
