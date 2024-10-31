using MedicalAssistant.Domain.DomainServices;
using MedicalAssistant.Domain.Policies;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalAssistant.Domain;
public static class Extensions
{
	public static IServiceCollection AddDomain(this IServiceCollection services)
		=> services
			.AddSingleton<IVisitPolicy, UserVisitPolicy>()
			.AddSingleton<IVisitService, VisitService>()
			.AddSingleton<IMedicationRecommendationPolicy,MedicationRecommendationPolicy>();
}
