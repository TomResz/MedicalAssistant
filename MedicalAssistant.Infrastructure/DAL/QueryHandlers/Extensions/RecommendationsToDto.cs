using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Extensions;
internal static class RecommendationsToDto
{
    internal static RecommendationDto ToDto(this Domain.Entities.MedicationRecommendation recommendation)
        => new RecommendationDto
        {
            Id = recommendation.Id,
            CreatedAt = recommendation.CreatedAt.ToDate(),
            ExtraNote = recommendation.ExtraNote,
            MedicineName = recommendation.Medicine.Name,
            MedicineQuantity = recommendation.Medicine.Quantity,
            MedicineTimeOfDay = recommendation.Medicine.TimeOfDay,
            BeginDate = recommendation.StartDate.ToDate(),
            EndDate = recommendation.EndDate.ToDate(),
        };
}
