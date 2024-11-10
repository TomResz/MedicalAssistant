using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.Repository;

internal sealed class MedicalHistoryRepository
    : IMedicalHistoryRepository
{
    private readonly MedicalAssistantDbContext _context;

    public MedicalHistoryRepository(MedicalAssistantDbContext context)
        => _context = context;

    public void Add(MedicalHistory medicalHistory)
        => _context.MedicalHistories.Add(medicalHistory);

    public async Task<MedicalHistory?> GetByIdAsync(MedicalHistoryId id, CancellationToken cancellationToken = default)
        => await _context
            .MedicalHistories
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public void Update(MedicalHistory medicalHistory)
        => _context.MedicalHistories.Update(medicalHistory);
}