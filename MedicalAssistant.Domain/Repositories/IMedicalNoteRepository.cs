using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Repositories;

public interface IMedicalNoteRepository
{
    void Add(MedicalNote medicalNote);
    Task<MedicalNote?> GetById(MedicalNoteId requestId, CancellationToken cancellationToken = default);
    void Update(MedicalNote medicalNote);
    void Delete(MedicalNote medicalNote);
}