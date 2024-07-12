using Hangfire;
using Hangfire.PostgreSql;
using HangfireBasicAuthenticationFilter;
using MedicalAssist.Infrastructure.BackgroundJobs;
using MedicalAssist.Infrastructure.Outbox;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalAssist.Infrastructure.BackgrounJobs;
public static class Extensions
{
    private const string databaseConnectionSection = "postgres:connectionString";
    private const string hangfireUserSection = "Hangfire:User";
    private const string hangfirePasswordSection = "Hangfire:Password";

    internal static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(cfg =>
        {
            cfg.UsePostgreSqlStorage(opt =>
                opt.UseNpgsqlConnection(configuration[databaseConnectionSection] ?? throw new ArgumentNullException()));
        });

        services.AddHangfireServer(opt => opt.SchedulePollingInterval = TimeSpan.FromSeconds(5));

        services.AddScoped<IProcessOutboxMessagesJob, ProcessOutboxMessagesJob>();
        return services;
    }

    public static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseHangfireDashboard("/hangfire", options: new DashboardOptions
        {
            Authorization = new[]
            {
                new HangfireCustomBasicAuthenticationFilter
                {
                    User = configuration[hangfireUserSection] ?? throw new ArgumentNullException(hangfireUserSection),
                    Pass = configuration[hangfirePasswordSection] ?? throw new ArgumentNullException(hangfirePasswordSection)
                }
            }
        });
        return app;
    }
}
