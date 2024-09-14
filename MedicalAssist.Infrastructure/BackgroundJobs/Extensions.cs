using Hangfire;
using Hangfire.PostgreSql;
using HangfireBasicAuthenticationFilter;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Infrastructure.BackgroundJobs;
using MedicalAssist.Infrastructure.DAL.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MedicalAssist.Infrastructure.BackgrounJobs;
public static class Extensions
{
    private const string hangfireUserSection = "Hangfire:User";
    private const string hangfirePasswordSection = "Hangfire:Password";
	internal static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
    {

		services.AddHangfire((sp,cfg) =>
		{
            var options = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;

			var isRunningInDocker = Environment.GetEnvironmentVariable("RUNNING_IN_DOCKER") == "true";
			var connectionString = isRunningInDocker ? options.DockerConnectionString : options.ConnectionString;

			cfg.UsePostgreSqlStorage(opt =>
                opt.UseNpgsqlConnection(connectionString ?? throw new ArgumentNullException()));

            cfg.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
               .UseSimpleAssemblyNameTypeSerializer()
               .UseRecommendedSerializerSettings();
        });

        services.AddHangfireServer(opt => opt.SchedulePollingInterval = TimeSpan.FromSeconds(5));

        services.AddSingleton<IEventPublisher, EventPublisher>();
        services.AddSingleton<IVisitNotificationScheduler, VisitNotificationScheduler>();
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
