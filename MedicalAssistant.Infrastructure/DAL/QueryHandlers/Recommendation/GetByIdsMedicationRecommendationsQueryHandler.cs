using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Application.MedicationRecommendations.Queries;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Recommendation;

internal sealed class GetByIdsMedicationRecommendationsQueryHandler
    : IQueryHandler<GetByIdsMedicationRecommendationsQuery,IEnumerable<MedicationRecommendationDto>>
{
    private readonly IUserContext _userContext;
    private readonly MedicalAssistantDbContext _dbContext;
    
    public GetByIdsMedicationRecommendationsQueryHandler(
        IUserContext userContext,
        MedicalAssistantDbContext dbContext)
    {
        _userContext = userContext;
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<MedicationRecommendationDto>> Handle(GetByIdsMedicationRecommendationsQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetUserId;
        var response = await _dbContext
            .Recommendations
            .Include(x => x.Visit)
            .Where(x => x.UserId == userId && request.IDs.Contains(x.Id) )
            .Select(x => x.ToDto())
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return response;
    }
}