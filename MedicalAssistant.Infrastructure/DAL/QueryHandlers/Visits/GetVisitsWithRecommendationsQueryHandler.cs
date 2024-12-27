using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Visits.Queries;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Visits;

internal sealed class GetVisitsWithRecommendationsQueryHandler
    : IQueryHandler<GetVisitsWithRecommendationsQuery, IEnumerable<VisitWithRecommendationsDto>>
{
    private readonly IUserContext _userContext;
    private readonly MedicalAssistantDbContext _context;

    public GetVisitsWithRecommendationsQueryHandler(
        IUserContext userContext,
        MedicalAssistantDbContext context)
    {
        _userContext = userContext;
        _context = context;
    }

    public async Task<IEnumerable<VisitWithRecommendationsDto>> Handle(
        GetVisitsWithRecommendationsQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetUserId;

        var response = await _context.Visits
            .Include(x => x.Recommendations)
            .Where(x=>x.UserId == userId && request.Ids.Contains(x.Id))
            .Select(x => new VisitWithRecommendationsDto
            {
                Date = (DateTime)x.Date,
                DoctorName = x.DoctorName,
                VisitType = x.VisitType,
                VisitDescription = x.VisitDescription,
                Recommendations = x.Recommendations.Select(r=> new RecommendationDto
                {
                    Id = r.Id,
                    BeginDate = (DateTime)r.StartDate,
                    EndDate = (DateTime)r.EndDate,
                    CreatedAt = (DateTime)r.CreatedAt,
                    ExtraNote = r.ExtraNote,
                    MedicineName = r.Medicine.Name,
                    MedicineQuantity = r.Medicine.Quantity,
                    MedicineTimeOfDay = r.Medicine.TimeOfDay,
                }).ToList()
            })
            .OrderBy(x=>x.Date)
            .ToListAsync(cancellationToken);

        return response;
    }
}