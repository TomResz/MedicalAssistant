using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace MedicalAssist.Infrastructure.Auth;
internal static class Extensions
{
    private const string OptionsSectionName = "auth";
    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        AuthOptions options = configuration.GetOptions<AuthOptions>(OptionsSectionName);
		services.Configure<AuthOptions>(configuration.GetSection(OptionsSectionName));
        services.AddAuthorizationBuilder()
            .AddPolicy(CustomClaim.IsVerified, b => b.AddRequirements(new UserVerification(true)))
            .AddPolicy(CustomClaim.IsNotVerified, b => b.AddRequirements(new UserVerification(false)));
        services.AddScoped<IAuthorizationHandler,UserVerificationHandler>();

		services
			.AddSingleton<IAuthenticator, Authenticator>()
            .AddScoped<IUserContext, UserContext>()
            .AddHttpContextAccessor()
            .AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.Audience = options.Audience;
                o.IncludeErrorDetails = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = options.Issuer,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SignInKey))
                };
            });
        return services;
    }
}
