using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Repositories;

public interface IMedicalHistoryRepository
{
    void Add(Domain.Entities.MedicalHistory medicalHistory);
    Task<MedicalHistory?> GetByIdAsync(MedicalHistoryId id,CancellationToken cancellationToken = default);
    void Update(MedicalHistory medicalHistory);
}