namespace MedicalAssistant.Domain.Exceptions;

public sealed class EmptyDiseaseStageNameException() 
    : BadRequestException("Stage name cannot be empty.");
    
public sealed class InvalidDiseaseStageNameLenghtException(int length)
    : BadRequestException($"Stage name lenght cannot be greater than {length}.");