using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.MedicationRecommendations.Queries;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Recommendation;
internal sealed class GetMedicationRecommendationQueryHandler
	: IQueryHandler<GetMedicationRecommendationQuery, MedicationRecommendationDto>
{
	private readonly MedicalAssistantDbContext _context;
	private readonly IUserContext _userContext;
	public GetMedicationRecommendationQueryHandler(
		MedicalAssistantDbContext context,
		IUserContext userContext)
	{
		_context = context;
		_userContext = userContext;
	}

	public async Task<MedicationRecommendationDto> Handle(GetMedicationRecommendationQuery request, CancellationToken cancellationToken)
	{
		var userId = _userContext.GetUserId;

		var response = await _context
			.Recommendations
			.Include(x=>x.Visit)
			.Where(x=>x.Id == new MedicationRecommendationId(request.Id) &&
						x.UserId == userId)
			.Select(x=> x.ToDto())
			.AsNoTracking()
			.FirstOrDefaultAsync(cancellationToken);
		return response ?? throw new MedicationRecommendationNotFoundException(request.Id);
	}
}
