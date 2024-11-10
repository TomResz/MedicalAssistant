using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;

public class UnknownMedicalHistoryException(Guid requestMedicalHistoryId)
    : BadRequestException($"Medical history with id {requestMedicalHistoryId} was not found.");