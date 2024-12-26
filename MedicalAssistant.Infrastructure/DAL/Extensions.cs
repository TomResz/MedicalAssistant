using Dapper;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Infrastructure.DAL.Dapper;
using MedicalAssistant.Infrastructure.DAL.Interceptors;
using MedicalAssistant.Infrastructure.DAL.Options;
using MedicalAssistant.Infrastructure.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MedicalAssistant.Infrastructure.DAL;
internal static class Extensions
{
	private const string OptionsSectionName = "postgres";
	internal static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
	{
        services.ConfigureOptions<DatabaseOptionsConfiguration>();

        services.AddSingleton<IDatabaseCreator,DatabaseCreator>();
		services.AddSingleton<DomainEventPublisherInterceptor>();

		services.AddDbContext<MedicalAssistantDbContext>((sp,opt )=>
		{
			var interceptor = sp.GetRequiredService<DomainEventPublisherInterceptor>();
			var connectionString = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value.ConnectionString;

			opt.UseNpgsql(connectionString)
				.AddInterceptors(interceptor)
				.ConfigureWarnings(builder =>
				{
					builder.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning);
				});
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
		services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
		services.AddScoped<IMedicationRecommendationNotificationRepository, MedicationRecommendationNotificationRepository>();
		services.AddScoped<IMedicalHistoryRepository, MedicalHistoryRepository>();
		services.AddScoped<IMedicalNoteRepository, MedicalNoteRepository>();
		
		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

		services.AddSingleton<ISqlConnectionFactory, ConnectionFactory>();
		SqlMapper.AddTypeHandler(new SqlTimeOnlyMapper());
		
		return services;
	}
}
