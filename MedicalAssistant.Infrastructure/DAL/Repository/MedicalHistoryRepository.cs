using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Infrastructure.DAL.Repository;

internal sealed class MedicalHistoryRepository
    : IMedicalHistoryRepository
{
    private readonly MedicalAssistantDbContext _context;

    public MedicalHistoryRepository(MedicalAssistantDbContext context)
    {
        _context = context;
    }

    public void Add(MedicalHistory medicalHistory)
    {
        _context.MedicalHistories.Add(medicalHistory);
    }
}