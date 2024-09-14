using Microsoft.Extensions.DependencyInjection;

namespace MedicalAssistant.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssemblies(typeof(Extensions).Assembly);
        });
		return services;
    }
}
