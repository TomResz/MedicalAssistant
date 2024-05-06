using MediatR;
using MedicalAssist.Application.Dto;
using MedicalAssist.Application.Recommendations.Queries;
using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;
using MedicalAssist.Infrastructure.DAL.QueryHandlers.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssist.Infrastructure.DAL.QueryHandlers.Recommendation;
internal sealed class GetRecommendationsForVisitQueryHandler : IRequestHandler<GetRecommendationsForVisitQuery, IEnumerable<RecommendationDto>>
{
    private readonly MedicalAssistDbContext _context;
    public GetRecommendationsForVisitQueryHandler(MedicalAssistDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<RecommendationDto>> Handle(GetRecommendationsForVisitQuery request, CancellationToken cancellationToken)
    {
        var recommendations = await _context
                .Recommendations
                .Where(x => x.VisitId == new VisitId(request.VisitId))
                .Select(x => x.ToDto())
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        return recommendations
            .OrderBy(x => Array.IndexOf(TimeOfDay.AvailableTimesOfDay.ToArray(), x.MedicineTimeOfDay));
    }
}
