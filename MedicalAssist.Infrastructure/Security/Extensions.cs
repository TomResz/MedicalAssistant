using MedicalAssist.Application.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalAssist.Infrastructure.Security;
internal static class Extensions
{
    internal static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services
            .AddSingleton(typeof(IPasswordHasher<>), typeof(PasswordHasher<>))
            .AddSingleton<IEmailCodeManager, EmailCodeManager>()
            .AddSingleton<IPasswordManager, PasswordManager>()
            .AddTransient<ICodeVerification, CodeVerification>();
        return services;
    }
}
