using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace MedicalAssistant.API.Endpoints;

public static class Extensions
{
	public static IServiceCollection AddEndpoints(this IServiceCollection services,Assembly assembly)
	{
		var servicesDescriptors = assembly
			.DefinedTypes
			.Where(type => type is { IsAbstract: false, IsInterface: false }
				&& type.IsAssignableTo(typeof(IEndpoints)))
			.Select(type => ServiceDescriptor.Transient(typeof(IEndpoints), type))
			.ToArray();
		services.TryAddEnumerable(servicesDescriptors);
		return services;
	}

	public static IApplicationBuilder MapEndpoints(this WebApplication app,RouteGroupBuilder? routeGroupBuilder = null)
	{
		var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoints>>();
		IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder; 

		foreach (var endpoint in endpoints)
		{
			endpoint.MapEndpoints(builder);
		}
		
		return app;
	}
}
