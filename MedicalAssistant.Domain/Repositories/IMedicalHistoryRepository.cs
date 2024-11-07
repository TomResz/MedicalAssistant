namespace MedicalAssistant.Domain.Repositories;

public interface IMedicalHistoryRepository
{
    void Add(Domain.Entities.MedicalHistory medicalHistory);
}