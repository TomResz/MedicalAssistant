using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.Visits.Commands.DeleteVisit;
public sealed record DeleteVisitCommand(
	Guid VisitId) : ICommand;