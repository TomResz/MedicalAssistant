namespace MedicalAssistant.Domain.Exceptions.IDs;

public sealed class InvalidDiseaseStageIdException() : BadRequestException("Id cannot be empty.");
