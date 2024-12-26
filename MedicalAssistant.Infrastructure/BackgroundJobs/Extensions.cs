using Hangfire;
using Hangfire.PostgreSql;
using HangfireBasicAuthenticationFilter;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Infrastructure.BackgroundJobs.RecurringJobs;
using MedicalAssistant.Infrastructure.DAL.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MedicalAssistant.Infrastructure.BackgroundJobs;
public static class Extensions
{
    private const string HangfireUserSection = "Hangfire:User";
    private const string HangfirePasswordSection = "Hangfire:Password";
	internal static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
    {

		services.AddHangfire((sp,cfg) =>
		{
            var options = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;

			var isRunningInDocker = Environment.GetEnvironmentVariable("RUNNING_IN_DOCKER") == "true";
			var connectionString = options.ConnectionString;

			cfg.UsePostgreSqlStorage(opt =>
                opt.UseNpgsqlConnection(connectionString ?? throw new ArgumentNullException()));

            cfg.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
               .UseSimpleAssemblyNameTypeSerializer()
               .UseRecommendedSerializerSettings();
        });

        services.AddHangfireServer(opt => opt.SchedulePollingInterval = TimeSpan.FromSeconds(5));

        services.AddSingleton<IEventPublisher, EventPublisher>();
        services.AddSingleton<IVisitNotificationScheduler, VisitNotificationScheduler>();
        services.AddSingleton<IMedicationRecommendationNotificationScheduler,MedicationNotificationScheduler>();
        services.AddScoped<IExpiredTokenRemovalJob,ExpiredTokenRemovalJob>();
        services.AddScoped<IUnverifiedAccountRemovalJob,UnverifiedAccountRemovalJob>();
        return services;
    }

    public static IApplicationBuilder UseRecurringBackgroundJobs(this WebApplication app)
    {
        app.Services
            .GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<IExpiredTokenRemovalJob>(
                "expired-token-deletion-job",
                    job => job.ProcessAsync(CancellationToken.None),
                    "*/5 * * * *");
        
        app.Services
            .GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<IUnverifiedAccountRemovalJob>(
                "unverified-accounts-deletion-job",
                job => job.ProcessAsync(CancellationToken.None),
                "*/10 * * * *");
        return app;
	}


    public static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseHangfireDashboard("/hangfire", options: new DashboardOptions
        {
            Authorization =
            [
                new HangfireCustomBasicAuthenticationFilter
                {
                    User = configuration[HangfireUserSection] ?? throw new ArgumentNullException(HangfireUserSection),
                    Pass = configuration[HangfirePasswordSection] ?? throw new ArgumentNullException(HangfirePasswordSection)
                }
            ]
        });
        return app;
    }
}
