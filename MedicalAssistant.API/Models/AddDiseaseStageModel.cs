namespace MedicalAssistant.API.Models;

public record AddDiseaseStageModel(
    Guid? VisitId,
    string? Note,
    string Name,
    DateTime Date);