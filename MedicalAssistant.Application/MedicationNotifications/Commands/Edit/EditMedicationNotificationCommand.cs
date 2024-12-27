using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.MedicationNotifications.Commands.Edit;
public sealed record EditMedicationNotificationCommand(
	Guid Id,
	DateTime Start,
	DateTime End,
	TimeOnly TriggerTimeUtc) : ICommand;