using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Visits.Commands.AddVisit;
public sealed record AddVisitCommand(
    string City,
    string PostalCode,
    string Street,
    string Date,
    string DoctorName,
    string VisitType,
    string VisitDescription,
    string PredictedEndDate) : ICommand<VisitDto>;
