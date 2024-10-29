using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Infrastructure.Auth;
using MedicalAssistant.Infrastructure.BackgrounJobs;
using MedicalAssistant.Infrastructure.DAL;
using MedicalAssistant.Infrastructure.Docker;
using MedicalAssistant.Infrastructure.Email;
using MedicalAssistant.Infrastructure.ExternalProviders;
using MedicalAssistant.Infrastructure.Language;
using MedicalAssistant.Infrastructure.Middleware;
using MedicalAssistant.Infrastructure.Notifications;
using MedicalAssistant.Infrastructure.Security;
using MedicalAssistant.Infrastructure.Time;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalAssistant.Infrastructure;
public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddMiddleware()
            .AddSingleton<IDockerChecker,DockerChecker>()
            .AddPersistance(configuration)
            .AddAuth(configuration)
            .AddSingleton<IClock, Clock>()
            .AddSecurity(configuration)
            .AddMediatR(conf =>
            {
                conf.RegisterServicesFromAssemblies(typeof(Extensions).Assembly);
            })
            .AddBackgroundJobs(configuration)
            .AddEmailServices(configuration)
            .AddGoogleService(configuration)
            .AddLanguageService()
            .AddSingleton<INotificationSender,NotificationSender>();

    internal static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        var section = configuration.GetRequiredSection(sectionName);
        section.Bind(options);

        return options;
    }
}
