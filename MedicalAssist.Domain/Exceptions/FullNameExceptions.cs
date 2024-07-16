using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.Exceptions;
public sealed class InvalidFullNameException : BadRequestException
{
	public InvalidFullNameException(string fullName) : base($"Full name: {fullName} is invalid.")
	{

	}
}
public sealed class SameFullNamesException : BadRequestException
{
	public SameFullNamesException() : base("New full name cannot be the same.")
	{

	}
}
