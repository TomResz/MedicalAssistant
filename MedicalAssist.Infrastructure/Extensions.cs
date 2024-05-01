using MedicalAssist.Application.Security;
using MedicalAssist.Domain.Abstraction;
using MedicalAssist.Infrastructure.Auth;
using MedicalAssist.Infrastructure.BackgrounJobs;
using MedicalAssist.Infrastructure.DAL;
using MedicalAssist.Infrastructure.Email;
using MedicalAssist.Infrastructure.Middleware;
using MedicalAssist.Infrastructure.Security;
using MedicalAssist.Infrastructure.Time;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalAssist.Infrastructure;
public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddMiddleware()
            .AddPersistance(configuration)
            .AddAuth(configuration)
            .AddSingleton<IPasswordManager, PasswordManager>()
            .AddSingleton<IClock, Clock>()
            .AddSingleton(typeof(IPasswordHasher<>), typeof(PasswordHasher<>))
            .AddTransient<ICodeVerification,CodeVerification>()
            .AddMediatR(conf =>
		    {
			    conf.RegisterServicesFromAssemblies(typeof(Extensions).Assembly);
		    })
            .AddBackgroundJobs()
            .AddEmailServices(configuration);


    internal static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        var section = configuration.GetRequiredSection(sectionName);
        section.Bind(options);

        return options;
    }
}
