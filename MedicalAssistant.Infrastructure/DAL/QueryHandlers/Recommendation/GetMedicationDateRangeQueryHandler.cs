using MediatR;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.MedicationRecommendations.Queries;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Recommendation;

internal sealed class GetMedicationDateRangeQueryHandler
	: IRequestHandler<GetMedicationDateRangeQuery, DateRangeDto?>
{
	private readonly MedicalAssistantDbContext _context;

	public GetMedicationDateRangeQueryHandler(MedicalAssistantDbContext context)
	{
		_context = context;
	}

	public async Task<DateRangeDto?> Handle(GetMedicationDateRangeQuery request, CancellationToken cancellationToken)
	{
		var response = await _context
			.Recommendations
			.Where(x=>x.Id == new MedicationRecommendationId(request.Id))
			.Select(x => new DateRangeDto
			{
				Start = (DateTime)x.StartDate,
				End = (DateTime)x.EndDate
			})
			.FirstOrDefaultAsync(cancellationToken);
		return response;	
	}
}
