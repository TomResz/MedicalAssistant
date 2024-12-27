using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Domain.ValueObjects.IDs;
using MedicalAssistant.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Application.MedicationNotifications.Queries;
internal sealed class GetMedicationNotificationByMedicationQueryHandler
	: IQueryHandler<GetMedicationNotificationByMedicationQuery, IEnumerable<MedicationNotificationWithDateRangeDto>>
{
	private readonly MedicalAssistantDbContext _context;
	public GetMedicationNotificationByMedicationQueryHandler(
		MedicalAssistantDbContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<MedicationNotificationWithDateRangeDto>> Handle(GetMedicationNotificationByMedicationQuery request, CancellationToken cancellationToken)
	{
		var medicationId = new MedicationRecommendationId(request.MedicationId);

		var response = await _context
			.MedicationRecommendationsNotifications
			.AsNoTracking()
			.Where(x => x.MedicationRecommendationId == medicationId)
			.Select(x => new MedicationNotificationWithDateRangeDto
			{
				Id = x.Id,
				StartDate = (DateTime)x.Start,
				EndDate = (DateTime)x.End,
				MedicationId = medicationId,
				Time = x.TriggerTimeUtc.AddHours(request.Offset),
			})
			.OrderBy(x=>x.StartDate)
			.ThenBy(x=>x.Time)
			.ToListAsync(cancellationToken: cancellationToken);
		
		return response;
	}
}
