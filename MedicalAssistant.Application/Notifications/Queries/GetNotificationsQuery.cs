using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Notifications.Queries;

public sealed record GetNotificationsQuery()
	: IQuery<IEnumerable<NotificationDto>>;
