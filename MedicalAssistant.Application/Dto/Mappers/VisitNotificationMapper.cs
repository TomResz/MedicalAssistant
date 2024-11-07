using MedicalAssistant.Domain.Entities;

namespace MedicalAssistant.Application.Dto.Mappers;
public static class VisitNotificationMapper
{
	public static VisitNotificationDto ToDto(this VisitNotification notification)
		=> new VisitNotificationDto
		{
			Id = notification.Id,
			ScheduledDateUtc = notification.ScheduledDateUtc.ToDate(),
			VisitId = notification.VisitId,
		};
}
