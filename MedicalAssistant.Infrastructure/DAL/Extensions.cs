using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Infrastructure.DAL.Interceptors;
using MedicalAssistant.Infrastructure.DAL.Options;
using MedicalAssistant.Infrastructure.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalAssistant.Infrastructure.DAL;
internal static class Extensions
{
	private const string OptionsSectionName = "postgres";
	internal static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
	{
		var postgresOptions = configuration.GetOptions<DatabaseOptions>(OptionsSectionName);
		services.Configure<DatabaseOptions>(configuration.GetSection(OptionsSectionName));

		var isRunningInDocker = Environment.GetEnvironmentVariable("RUNNING_IN_DOCKER") == "true";

		var connectionString = isRunningInDocker ? postgresOptions.DockerConnectionString : postgresOptions.ConnectionString;
		
		services.AddSingleton<IDatabaseCreator,DatabaseCreator>();
		services.AddSingleton<DomainEventPublisherInterceptor>();

		services.AddDbContext<MedicalAssistantDbContext>((sp,opt )=>
		{
			var interceptor = sp.GetRequiredService<DomainEventPublisherInterceptor>();
			
			opt
			.UseNpgsql(connectionString)
			.AddInterceptors(interceptor);
		});
		services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IVisitRepository, VisitRepository>();
		services.AddScoped<IVisitNotificationRepository, VisitNotificationRepository>();
		services.AddScoped<INotificationHistoryRepository, NotificationHistoryRepository>();
		services.AddScoped<ITokenRepository, TokenRepository>();
		services.AddScoped<IAttachmentRepository, AttachmentRepository>();
		services.AddScoped<IUserSettingRepository,UserSettingsRepository>();
		services.AddScoped<IMedicationRecommendationRepository, MedicationRecommendationRepository>();

		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

		return services;
	}
}
