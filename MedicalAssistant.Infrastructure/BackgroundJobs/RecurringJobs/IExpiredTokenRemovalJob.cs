
namespace MedicalAssistant.Infrastructure.BackgroundJobs.RecurringJobs;

public interface IExpiredTokenRemovalJob
{
	Task ProcessAsync();
}