using MediatR;
using MedicalAssist.Application.Dto;
using MedicalAssist.Application.Recommendations.Queries;
using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;
using MedicalAssist.Infrastructure.DAL.QueryHandlers.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssist.Infrastructure.DAL.QueryHandlers.Recommendation;
internal sealed class GetRecommendationsForGivenTimePeriodQueryHandler
    : IRequestHandler<GetRecommendationsForGivenTimePeriodQuery, IEnumerable<RecommendationDto>>
{
    private readonly MedicalAssistDbContext _context;
    public GetRecommendationsForGivenTimePeriodQueryHandler(MedicalAssistDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RecommendationDto>> Handle(GetRecommendationsForGivenTimePeriodQuery request, CancellationToken cancellationToken)
    {
        Date begin = request.Begin.Date;
        Date end = request.End.Date;

        var recommendations = await _context
            .Recommendations
            .Where(x=>x.VisitId == new VisitId(request.VisitId)
                && x.StartDate <= end
                && x.EndDate >= begin)
            .Select(x=>x.ToDto())
            .ToListAsync(cancellationToken);

        return recommendations;
    }
}
