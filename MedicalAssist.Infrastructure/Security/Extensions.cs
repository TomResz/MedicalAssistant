using MedicalAssist.Application.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalAssist.Infrastructure.Security;
internal static class Extensions
{
    internal static IServiceCollection AddSecurity(this IServiceCollection services,IConfiguration configuration)
    {
        services.Configure<AESOptions>(configuration.GetSection("AES"));
		services
            .AddSingleton(typeof(IPasswordHasher<>), typeof(PasswordHasher<>))
			.AddSingleton<IEmailCodeManager, EmailCodeManager>()
            .AddSingleton<IPasswordManager, PasswordManager>()
            .AddTransient<ICodeVerification, CodeVerification>();

        services.AddSingleton<IAESService, AESService>();

		return services;
    }
}
