using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.ValueObjects.IDs;
using MedicalAssistant.Domain.ValueObjects;

namespace MedicalAssistant.Application.Tests.ObjectFactories;
public class RecommendationFactory
{
	public static Recommendation Create(
		VisitId visitId,
		Note? extraNote = null,
		Date? createdAt = null,
		Medicine? medicine = null,
		Date? startDate = null,
		Date? endDate = null)
	{
		extraNote ??= "Note";
		createdAt ??= DateTime.Now;
		medicine ??= new("Aspiryna",1, TimeOfDay.Morning);
		startDate ??= DateTime.Now;
		endDate ??= DateTime.Now.AddDays(3);

		return Recommendation.Create(
			visitId,
			 extraNote,
			 createdAt,
			 medicine,
			 startDate,
			 endDate);
	}
}
