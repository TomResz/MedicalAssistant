using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Security;
using MedicalAssist.Infrastructure.Auth.AuthPolicy;
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
        
        services
            .AddScoped<IAuthorizationHandler, UserVerificationHandler>()
            .AddTransient<IRefreshTokenService, RefreshTokenService>();

        services
            .AddSingleton<IAuthenticator, Authenticator>()
            .AddScoped<IUserContext, UserContext>()
            .AddHttpContextAccessor()
            .AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
            {
                o.Audience = options.Audience;
                o.IncludeErrorDetails = true;
                o.SaveToken = true;
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = options.Issuer,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SignInKey))
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(CustomClaim.IsVerified, b => b.AddRequirements(new UserVerification(true)));
            options.AddPolicy(CustomClaim.IsNotVerified, b => b.AddRequirements(new UserVerification(false)));
            options.AddPolicy(CustomClaim.HasExternalProvider, b => b.AddRequirements(new UserExternalProvider(true)));
            options.AddPolicy(CustomClaim.HasInternalProvider, b => b.AddRequirements(new UserExternalProvider(false)));

		});
        return services;
    }
}
