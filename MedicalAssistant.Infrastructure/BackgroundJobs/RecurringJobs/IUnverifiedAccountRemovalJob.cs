namespace MedicalAssistant.Infrastructure.BackgroundJobs.RecurringJobs;

public interface IUnverifiedAccountRemovalJob
{
    Task ProcessAsync(CancellationToken cancellationToken);
}