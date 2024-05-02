using MedicalAssist.Infrastructure.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace MedicalAssist.Infrastructure.BackgrounJobs;
internal static class Extensions
{
	internal static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
		=> services.AddQuartz(conf =>
		{
			var jobKey = JobKey.Create(nameof(ProcessOutboxMessagesJob));
			conf
				.AddJob<ProcessOutboxMessagesJob>(jobKey)
				.AddTrigger(
				trigger =>
					trigger.ForJob(jobKey).WithSimpleSchedule(
						schedule => schedule.WithIntervalInSeconds(15).RepeatForever())
			);
			conf.UseMicrosoftDependencyInjectionJobFactory();
		})
		.AddQuartzHostedService(settings => settings.WaitForJobsToComplete = true);
}
