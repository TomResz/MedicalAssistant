using MedicalAssist.Domain.DomainServices;
using MedicalAssist.Domain.Policies;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalAssist.Domain;
public static class Extensions
{
	public static IServiceCollection AddDomain(this IServiceCollection services)
		=> services
			.AddSingleton<IVisitPolicy, UserVisitPolicy>()
			.AddSingleton<IVisitService, VisitService>();
}
