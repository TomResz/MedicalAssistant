using MedicalAssist.Domain.ComplexTypes;
using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.ValueObjects.IDs;
using MedicalAssist.Domain.ValueObjects;

namespace MedicalAssist.Application.Tests.ObjectFactories;
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
