
using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.MedicationNotifications.Commands.Delete;
public  sealed record DeleteMedicationNotificationCommand(
	Guid Id) : ICommand;
