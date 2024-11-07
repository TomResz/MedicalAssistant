using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.Repository;
internal sealed class MedicationRecommendationNotificationRepository
	: IMedicationRecommendationNotificationRepository
{
	private readonly MedicalAssistantDbContext _context;

	public MedicationRecommendationNotificationRepository(MedicalAssistantDbContext context)
	{
		_context = context;
	}

	public async Task<int> DeleteAsync(MedicationRecommendationNotificationId id, CancellationToken ct = default)
	{
		return await _context
			.MedicationRecommendationsNotifications
			.Where(x=>x.Id == id)
			.ExecuteDeleteAsync(ct);
	}

	public void Update(MedicationRecommendationNotification notification) 
		=> _context
		   .MedicationRecommendationsNotifications
		   .Update(notification);
}
