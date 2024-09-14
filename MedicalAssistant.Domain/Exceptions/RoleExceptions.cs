using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions;
public sealed class InvalidRoleException : BadRequestException
{
	public InvalidRoleException(string role) : base($"Role: {role} is invalid.")
	{
	}
}
