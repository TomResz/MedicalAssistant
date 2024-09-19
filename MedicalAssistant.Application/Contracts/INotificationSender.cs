using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Application.Contracts;
public interface INotificationSender
{
	Task SendNotification(UserId userId,string content);
}
