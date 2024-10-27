using MedicalAssistant.Domain.Entites;

namespace MedicalAssistant.Application.Dto.Mappers;
public static class MedicationRecommendationToDto
{
	public static MedicationRecommendationDto ToDto(this MedicationRecommendation recommendation)
		=> new()
		{
			Id = recommendation.Id,
			EndDate = recommendation.EndDate.ToDate(),
			StartDate = recommendation.StartDate.ToDate(),
			ExtraNote = recommendation.ExtraNote,
			Name = recommendation.Medicine.Name,
			Quantity = recommendation.Medicine.Quantity,
			TimeOfDay = recommendation.Medicine.TimeOfDay,
			Visit = recommendation.Visit == null ? null : recommendation.Visit.ToDto()
		};
}
