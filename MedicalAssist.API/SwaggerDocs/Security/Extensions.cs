namespace MedicalAssist.API.SwaggerDocs.Security;

public static class Extensions
{
    public static IServiceCollection AddSwaggerAuthMiddleware(this IServiceCollection services)
        => services.AddScoped<SwaggerAuthMiddleware>();
    public static IApplicationBuilder UseSwaggerAuthMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<SwaggerAuthMiddleware>();
}

