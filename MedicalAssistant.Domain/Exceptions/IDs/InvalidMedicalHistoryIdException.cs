namespace MedicalAssistant.Domain.Exceptions.IDs;

public sealed class InvalidMedicalHistoryIdException() 
    : BadRequestException("Medical History Id cannot be empty!");