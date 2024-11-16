using MediatR;

namespace MedicalAssistant.Application.MedicalHistory.Commands.Delete;

public sealed record DeleteMedicalHistoryCommand(Guid Id) 
    : IRequest;