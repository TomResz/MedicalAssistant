using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MedicalAssistant.Infrastructure.BackgroundJobs.RecurringJobs;

internal sealed class UnverifiedAccountRemovalJob : IUnverifiedAccountRemovalJob
{
    private readonly MedicalAssistantDbContext _context;
    private readonly ILogger<UnverifiedAccountRemovalJob> _logger;
    private readonly IClock _clock;

    public UnverifiedAccountRemovalJob(
        MedicalAssistantDbContext context,
        ILogger<UnverifiedAccountRemovalJob> logger,
        IClock clock)
    {
        _context = context;
        _logger = logger;
        _clock = clock;
    }

    public async Task ProcessAsync(CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var now = _clock.GetCurrentUtc();
            var accountToDelete = await _context
                .Users
                .IgnoreQueryFilters()
                .Include(x=>x.UserVerification)
                .Where(x =>
                        !x.IsVerified && 
                        x.UserVerification!.ExpirationDate <= new Date(now) )
                .ToListAsync(cancellationToken);

            _context
                .Users
                .RemoveRange(accountToDelete);
            
            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            if (accountToDelete.Count > 0)
            {
                _logger.LogInformation("{Count} unverified accounts were deleted", accountToDelete.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while removing unverified accounts.");
            await transaction.RollbackAsync(cancellationToken);
        }
    }
}