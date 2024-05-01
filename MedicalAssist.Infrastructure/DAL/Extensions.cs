using MedicalAssist.Application.Contracts;
using MedicalAssist.Domain.Repositories;
using MedicalAssist.Infrastructure.DAL.Interceptors;
using MedicalAssist.Infrastructure.DAL.Options;
using MedicalAssist.Infrastructure.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalAssist.Infrastructure.DAL;
internal static class Extensions
{
	private const string OptionsSectionName = "postgres";
	internal static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
	{
		var postgresOptions = configuration.GetOptions<PostgresOptions>(OptionsSectionName);

		services
			.AddSingleton<ConvertDomainEventToOutboxMessageInterceptor>()
			.AddDbContext<MedicalAssistDbContext>((sp,opt )=>
		{
			var interceptor = sp.GetService<ConvertDomainEventToOutboxMessageInterceptor>();
			opt.UseNpgsql(postgresOptions.ConnectionString)
				.AddInterceptors(interceptor);
		});
		services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IVisitRepository, VisitRepository>();

		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

		return services;
	}
}
