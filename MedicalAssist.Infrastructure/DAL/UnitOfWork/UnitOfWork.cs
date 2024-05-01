using MedicalAssist.Application.Contracts;

namespace MedicalAssist.Infrastructure.DAL.UnitOfWork;
internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly MedicalAssistDbContext _context;

    public UnitOfWork(MedicalAssistDbContext context) => _context = context;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
