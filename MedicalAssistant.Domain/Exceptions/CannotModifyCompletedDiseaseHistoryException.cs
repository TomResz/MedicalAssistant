namespace MedicalAssistant.Domain.Exceptions;

public sealed class CannotModifyCompletedDiseaseHistoryException() : BadRequestException("Cannot modify disease history.");