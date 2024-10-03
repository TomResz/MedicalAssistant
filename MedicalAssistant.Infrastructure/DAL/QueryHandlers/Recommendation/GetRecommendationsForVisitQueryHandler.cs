using MediatR;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Recommendations.Queries;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;
using MedicalAssistant.Infrastructure.DAL.QueryHandlers.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Recommendation;
internal sealed class GetRecommendationsForVisitQueryHandler : IRequestHandler<GetRecommendationsForVisitQuery, IEnumerable<RecommendationDto>>
{
    private readonly MedicalAssistantDbContext _context;
    public GetRecommendationsForVisitQueryHandler(MedicalAssistantDbContext context)
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
