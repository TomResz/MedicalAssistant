using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalAssist.Infrastructure.BackgroundJobs;
public static class BackgroundJobExtensions
{
    public static IApplicationBuilder UseOutboxMessageProcessing(this WebApplication app)
    {
        app.Services
            .GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<IProcessOutboxMessagesJob>(
            "outbox-processing", j => j.ExecuteJob(default),
            "0/15 * * * * *");
        return app;
    }
}
