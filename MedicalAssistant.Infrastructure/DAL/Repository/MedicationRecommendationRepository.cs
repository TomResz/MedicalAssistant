using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.Repositories;
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

	public async Task<MedicationRecommendation?> GetWithNotificationsAsync(MedicationRecommendationId recommendationId, CancellationToken cancellationToken)
		=> await _context
		.Recommendations
		.Include(x=>x.Visit)
		.Include(x=>x.Notifications)
		.FirstOrDefaultAsync(x=>x.Id == recommendationId,cancellationToken);
}
