using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Application.MedicationRecommendations.Queries;
using MedicalAssistant.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Recommendation;

internal sealed class GetMedicationRecommendationByDateQueryHandler
    : IRequestHandler<GetMedicationRecommendationByDateQuery, IEnumerable<MedicationRecommendationDto>>
{
    private readonly MedicalAssistantDbContext _context;
    private readonly IUserContext _userContext;

    public GetMedicationRecommendationByDateQueryHandler(
        MedicalAssistantDbContext context,
        IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<IEnumerable<MedicationRecommendationDto>> Handle(
        GetMedicationRecommendationByDateQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetUserId;
        var date = new Date(request.Date.Date);

        var response = await _context
            .Recommendations
            .Include(x => x.Visit)
            .AsNoTracking()
            .Where(x => x.StartDate <= date &&
                        x.EndDate >= date &&
                        x.UserId == userId)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);
        return response;
    }
}