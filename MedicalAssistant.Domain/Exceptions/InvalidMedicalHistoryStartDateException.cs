namespace MedicalAssistant.Domain.Exceptions;

public sealed class InvalidMedicalHistoryStartDateException()
    : BadRequestException("Start date cannot be lesser than visit start date.");