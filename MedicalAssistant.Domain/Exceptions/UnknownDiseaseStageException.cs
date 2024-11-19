namespace MedicalAssistant.Domain.Exceptions;

public class UnknownDiseaseStageException(Guid stageId) 
    : BadRequestException($"Stage with Id='{stageId}' was not found.");