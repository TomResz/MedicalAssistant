using MediatR;

namespace MedicalAssistant.Application.MedicationNotifications.Commands.Delete;
public  sealed record DeleteMedicationNotificationCommand(
	Guid Id) : IRequest;
