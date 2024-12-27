using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Visits.Commands.EditVisit;
public sealed record EditVisitCommand(
	Guid Id,
	string City,
	string PostalCode,
	string Street,
	string Date,
	string DoctorName,
	string VisitType,
	string VisitDescription,
	string PredictedEndDate) : ICommand<VisitDto>;
