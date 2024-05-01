using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions;
public sealed class InvalidDateFormatException : BadRequestException
{
	public InvalidDateFormatException() : base("Invalid date format (valid format: yyyy-MM-dd HH:mm)!")
	{

	}
}
public sealed class SameDateException : BadRequestException
{
	public SameDateException() : base("Dates cannot be the same.")
	{

	}
}
