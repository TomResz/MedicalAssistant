using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Application.MedicationRecommendations.Queries;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Recommendation;

internal sealed class GetAllMedicationRecommendationsQueryHandler
	: IQueryHandler<GetAllMedicationRecommendationsQuery, IEnumerable<MedicationRecommendationDto>>
{
	private readonly IUserContext _userContext;
	private readonly MedicalAssistantDbContext _context;
	public GetAllMedicationRecommendationsQueryHandler(
		IUserContext userContext,
		MedicalAssistantDbContext context)
	{
		_userContext = userContext;
		_context = context;
	}

	public async Task<IEnumerable<MedicationRecommendationDto>> Handle(GetAllMedicationRecommendationsQuery request, CancellationToken cancellationToken)
	{
		var userId = _userContext.GetUserId;
		var response = await _context
			.Recommendations
			.Include(x => x.Visit)
			.Where(x => x.UserId == userId)
			.Select(x => x.ToDto())
			.AsNoTracking()
			.ToListAsync(cancellationToken);

		return response;
	}
}
