using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.ValueObjects.IDs;
using MedicalAssistant.Domain.ValueObjects;

namespace MedicalAssistant.Application.Tests.ObjectFactories;
public class RecommendationFactory
{
	public static MedicationRecommendation Create(
		VisitId visitId,
		UserId userId,
		Note? extraNote = null,
		Date? createdAt = null,
		Medicine? medicine = null,
		Date? startDate = null,
		Date? endDate = null)
	{
		extraNote ??= "Note";
		createdAt ??= DateTime.Now;
		medicine ??= new("Aspiryna",1, new string[] { TimeOfDay.Morning });
		startDate ??= DateTime.Now;
		endDate ??= DateTime.Now.AddDays(3);

		return MedicationRecommendation.Create(
			visitId,
			userId,
			 extraNote,
			 createdAt,
			 medicine,
			 startDate,
			 endDate);
	}
}
