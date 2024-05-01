using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions;
public class DoctorNameLengthExceededException : BadRequestException
{
	public DoctorNameLengthExceededException(int maxLength) : base($"The maximum doctor name length ({maxLength} characters) was exceeded.")
	{

	}
}
public sealed class EmptyDoctorNameException : BadRequestException
{
	public EmptyDoctorNameException() : base("Doctor name property cannot be empty.")
	{

	}
}
