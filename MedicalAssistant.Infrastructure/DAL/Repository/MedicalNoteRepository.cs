using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.Repository;

public sealed class MedicalNoteRepository : IMedicalNoteRepository
{
    private readonly MedicalAssistantDbContext _context;

    public MedicalNoteRepository(MedicalAssistantDbContext context) => _context = context;

    public void Add(MedicalNote medicalNote)
        => _context.MedicalNotes.Add(medicalNote);

    public async Task<MedicalNote?> GetById(MedicalNoteId requestId, CancellationToken cancellationToken = default)
        => await _context
            .MedicalNotes
            .FirstOrDefaultAsync(x => x.Id == requestId, cancellationToken);

    public void Update(MedicalNote medicalNote)
        => _context.MedicalNotes.Update(medicalNote);

    public void Delete(MedicalNote medicalNote) => _context.MedicalNotes.Remove(medicalNote);
}