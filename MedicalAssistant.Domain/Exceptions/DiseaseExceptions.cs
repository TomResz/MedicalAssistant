namespace MedicalAssistant.Domain.Exceptions;


public sealed class EmptyDiseaseNameException() : BadRequestException("Name cannot be empty.");

public sealed class InvalidDiseaseNameLenghtException(int maxLenght) 
    : BadRequestException($"Name cannot be greater than {maxLenght} characters.");