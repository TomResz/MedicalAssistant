using MedicalAssistant.Application.Contracts;

namespace MedicalAssistant.Infrastructure.DAL.UnitOfWork;
internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly MedicalAssistantDbContext _context;

    public UnitOfWork(MedicalAssistantDbContext context) => _context = context;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
