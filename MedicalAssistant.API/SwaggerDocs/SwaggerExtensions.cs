using MedicalAssistant.API.SwaggerDocs.CustomHeaders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace MedicalAssistant.API.SwaggerDocs;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDoc(this IServiceCollection services)
	{
        services.AddSwaggerGen(c =>
        {
			c.MapType<TimeOnly>(() => new OpenApiSchema
			{
				Type = "string",
				Format = "time",
				Example = new OpenApiString(DateTime.UtcNow.ToString("HH:mm"))
			});

			c.SwaggerDoc("v1", new OpenApiInfo { Title = "Medical Assistant API", Version = "v1" });
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT",
                Description = "Enter your JWT token",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, [] }
            });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
            c.OperationFilter<LanguageHeader>();
        });
        return services;
    }
}
