namespace MedicalAssist.Infrastructure.BackgroundJobs;
public interface IProcessOutboxMessagesJob
{
    Task ExecuteJob(CancellationToken cancellationToken = default);
}
