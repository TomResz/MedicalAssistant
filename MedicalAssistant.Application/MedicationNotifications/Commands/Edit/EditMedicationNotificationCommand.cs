using MediatR;

namespace MedicalAssistant.Application.MedicationNotifications.Commands.Edit;
public sealed record EditMedicationNotificationCommand(
	Guid Id,
	DateTime Start,
	DateTime End,
	TimeOnly TriggerTimeUtc) : IRequest;