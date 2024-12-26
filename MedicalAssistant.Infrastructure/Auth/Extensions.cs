using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Security;
using MedicalAssistant.Infrastructure.Auth.AuthPolicy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MedicalAssistant.Infrastructure.Auth.AuthPolicy.Handlers;
using MedicalAssistant.Infrastructure.Auth.Exceptions;


namespace MedicalAssistant.Infrastructure.Auth;

internal static class Extensions
{
    private const string OptionsSectionName = "auth";

    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        AuthOptions options = configuration.GetOptions<AuthOptions>(OptionsSectionName);
        services.Configure<AuthOptions>(configuration.GetSection(OptionsSectionName));

        services
            .AddScoped<IAuthorizationHandler, ActiveAndVerifiedUserProviderHandler>()
            .AddScoped<IAuthorizationHandler,NotActiveUserProviderHandler>()
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
                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/notifications")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy(CustomPolicy.IsVerifiedAndActive, b => b.AddRequirements(new ActiveAndVerifiedUserProvider(true, true)))
            .AddPolicy(CustomPolicy.NotActive, b => b.AddRequirements(new NotActiveUserProvider(false)));
        return services;
    }
}