using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Notifications.Queries;

public sealed record GetNotificationsQuery()
	: IRequest<IEnumerable<NotificationDto>>;
