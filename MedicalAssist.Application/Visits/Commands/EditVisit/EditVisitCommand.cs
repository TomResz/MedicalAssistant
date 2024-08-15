using MediatR;
using MedicalAssist.Application.Dto;

namespace MedicalAssist.Application.Visits.Commands.EditVisit;
public sealed record EditVisitCommand(
	Guid Id,
	string City,
	string PostalCode,
	string Street,
	string Date,
	string DoctorName,
	string VisitType,
	string VisitDescription,
	string PredictedEndDate) : IRequest<VisitDto>;
