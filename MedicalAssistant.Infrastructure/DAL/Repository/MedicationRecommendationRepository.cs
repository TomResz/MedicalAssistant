using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.Repository;

internal sealed class MedicationRecommendationRepository : IMedicationRecommendationRepository
{
	private readonly MedicalAssistantDbContext _context;

	public MedicationRecommendationRepository(MedicalAssistantDbContext context) 
		=> _context = context;

	public void Delete(MedicationRecommendation medicationRecommendation) 
		=> _context.Recommendations.Remove(medicationRecommendation);

	public async Task<MedicationRecommendation?> GetAsync(MedicationRecommendationId id, CancellationToken cancellationToken) 
		=> await _context
			.Recommendations
			.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

	public async Task<MedicationRecommendation?> GetByNotificationIdAsync(MedicationRecommendationNotificationId notificationId, CancellationToken cancellationToken)
	{
		return await _context
			.Recommendations
			.Include(x => x.Notifications)
			.FirstOrDefaultAsync(x => x.Notifications.Any(n => n.Id == notificationId), cancellationToken);
	}

	public async Task<MedicationRecommendation?> GetWithNotificationsAsync(MedicationRecommendationId recommendationId, CancellationToken cancellationToken)
		=> await _context
		.Recommendations
		.Include(x=>x.Visit)
		.Include(x=>x.Notifications)
		.FirstOrDefaultAsync(x=>x.Id == recommendationId,cancellationToken);


	public async Task<bool> ExistsUsageAsync(MedicationRecommendationId recommendationId, Date date,string TimeOfDay,CancellationToken cancellationToken)
	{
		var onlyDate = new Date(date.ToDate());
		var response = await _context
			.RecommendationUsages
			.AnyAsync(x => x.TimeOfDay == TimeOfDay &&
				x.Date == onlyDate &&
				x.MedicationRecommendationId == recommendationId, 
				cancellationToken: cancellationToken);

		return response;
	}


	public async Task<MedicationRecommendation?> GetWithUsagesAsync(MedicationRecommendationId recommendationId, CancellationToken cancellationToken)
		=> await _context
		.Recommendations
		.Include(x => x.Visit)
		.Include(x => x.RecommendationUsages)
		.FirstOrDefaultAsync(x => x.Id == recommendationId, cancellationToken);
}
