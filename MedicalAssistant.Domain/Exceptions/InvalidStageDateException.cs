namespace MedicalAssistant.Domain.Exceptions;

public sealed class InvalidStageDateException() 
    : BadRequestException("Stage date cannot be less than medical history date.");