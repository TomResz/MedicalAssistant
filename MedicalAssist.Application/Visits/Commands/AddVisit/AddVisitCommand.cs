using MediatR;
using MedicalAssist.Application.Dto;

namespace MedicalAssist.Application.Visits.Commands.AddVisit;
public sealed record AddVisitCommand(
    string City,
    string PostalCode,
    string Street,
    string Date,
    string DoctorName,
    string VisitType,
    string VisitDescription,
    string PredictedEndDate) : IRequest<VisitDto>;
