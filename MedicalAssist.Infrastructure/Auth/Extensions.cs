using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Text;


namespace MedicalAssist.Infrastructure.Auth;
internal static class Extensions
{
    private const string CustomAuthenticationSchema = "JwtOrCookie";
    private const string OptionsSectionName = "auth";
    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        AuthOptions options = configuration.GetOptions<AuthOptions>(OptionsSectionName);
		services.Configure<AuthOptions>(configuration.GetSection(OptionsSectionName));

        
        services.AddScoped<IAuthorizationHandler,UserVerificationHandler>();

        services
            .AddSingleton<IAuthenticator, Authenticator>()
            .AddScoped<IUserContext, UserContext>()
            .AddHttpContextAccessor()
            .AddAuthentication(o =>
            {
                o.DefaultScheme = CustomAuthenticationSchema;
                o.DefaultChallengeScheme = CustomAuthenticationSchema;
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
            })
            .AddCookie()
            .AddGoogle(options =>
            {
                options.ClientId = configuration["Authentication:Google:ClientId"] ?? throw new ArgumentNullException("ClientId");
                options.ClientSecret = configuration["Authentication:Google:ClientSecret"] ?? throw new ArgumentNullException("ClientSecret");
            }).AddPolicyScheme(CustomAuthenticationSchema, CustomAuthenticationSchema, options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    string? authorization = context.Request.Headers[HeaderNames.Authorization];
                    if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer ")) 
                    {
                        return JwtBearerDefaults.AuthenticationScheme;
                    }
                    return CookieAuthenticationDefaults.AuthenticationScheme;
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(CustomClaim.IsVerified, b => b.AddRequirements(new UserVerification(true)));
                options.AddPolicy(CustomClaim.IsNotVerified, b => b.AddRequirements(new UserVerification(false)));
            });
        return services;
    }
}
