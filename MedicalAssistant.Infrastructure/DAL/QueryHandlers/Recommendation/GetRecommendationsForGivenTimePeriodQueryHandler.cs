using MediatR;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.MedicationRecommendations.Queries;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;
using MedicalAssistant.Infrastructure.DAL.QueryHandlers.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Recommendation;
internal sealed class GetRecommendationsForGivenTimePeriodQueryHandler
    : IRequestHandler<GetRecommendationsForGivenTimePeriodQuery, IEnumerable<RecommendationDto>>
{
    private readonly MedicalAssistantDbContext _context;
    public GetRecommendationsForGivenTimePeriodQueryHandler(MedicalAssistantDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RecommendationDto>> Handle(GetRecommendationsForGivenTimePeriodQuery request, CancellationToken cancellationToken)
    {
        Date begin = request.Begin.Date;
        Date end = request.End.Date;

        var recommendations = await _context
            .Recommendations
            .Include(x=>x.Visit)
            .Where(x=>x.VisitId == new VisitId(request.VisitId)
                && x.StartDate <= end
                && x.EndDate >= begin)
            .Select(x=>x.ToDto())
            .ToListAsync(cancellationToken);

        return recommendations;
    }
}
