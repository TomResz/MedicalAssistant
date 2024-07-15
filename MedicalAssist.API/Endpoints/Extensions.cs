using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace MedicalAssist.API.Endpoints;

public static class Extensions
{
	public static IServiceCollection AddEndpoints(this IServiceCollection services,Assembly assembly)
	{
		var servicesDescriptors = assembly
			.DefinedTypes
			.Where(type => type is { IsAbstract: false, IsInterface: false }
				&& type.IsAssignableTo(typeof(IEndpoint)))
			.Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
			.ToArray();

		services.TryAddEnumerable(servicesDescriptors);
		return services;
	}

	public static IApplicationBuilder MapEndpoints(this WebApplication app,RouteGroupBuilder? routeGroupBuilder = null)
	{
		var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
		IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder; 

		foreach (var endpoint in endpoints)
		{
			endpoint.MapEndpoint(builder);
		}

		return app;
	}
}
