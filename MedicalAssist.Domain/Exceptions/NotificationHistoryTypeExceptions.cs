namespace MedicalAssist.Domain.Exceptions;

public sealed class EmptyNotificationHistoryTypeException : BadRequestException
{
	public EmptyNotificationHistoryTypeException() : base("Type cannot be empty or null.")
	{
	}
}