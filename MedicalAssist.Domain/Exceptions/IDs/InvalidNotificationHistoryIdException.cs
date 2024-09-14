namespace MedicalAssist.Domain.Exceptions.IDs;
public sealed class InvalidNotificationHistoryIdException : BadRequestException
{
	public InvalidNotificationHistoryIdException() : base("Invalid notification history Id.")
	{
	}
}
