using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MedicalAssistant.Infrastructure.BackgroundJobs.RecurringJobs;
internal sealed class ExpiredTokenRemovalJob : IExpiredTokenRemovalJob
{
	private readonly MedicalAssistantDbContext _context;
	private readonly IClock _clock;
	private readonly ILogger<ExpiredTokenRemovalJob> _logger;


	public ExpiredTokenRemovalJob(
		MedicalAssistantDbContext context,
		IClock clock,
		ILogger<ExpiredTokenRemovalJob> logger)
	{
		_context = context;
		_clock = clock;
		_logger = logger;
	}

	public async Task ProcessAsync()
	{
		using var transaction = await _context.Database.BeginTransactionAsync();
		try
		{
			var now = _clock.GetCurrentUtc();
			var deletedCount = await _context
				.TokenHolders
				.Where(x => x.RefreshTokenExpirationUtc < new Date(now))
				.ExecuteDeleteAsync();

			await _context.SaveChangesAsync();

			await transaction.CommitAsync();

			if (deletedCount > 0)
			{
				_logger.LogInformation("{Count} expired tokens were deleted", deletedCount);
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while removing expired tokens.");
			await transaction.RollbackAsync();
		}
	}
}
