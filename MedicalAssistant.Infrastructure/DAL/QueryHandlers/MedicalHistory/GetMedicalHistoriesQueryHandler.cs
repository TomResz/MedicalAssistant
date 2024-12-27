using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Application.MedicalHistory.Query;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.MedicalHistory;

internal sealed class GetMedicalHistoriesQueryHandler
    : IQueryHandler<GetMedicalHistoriesQuery, IEnumerable<MedicalHistoryDto>>
{
    private readonly MedicalAssistantDbContext _context;
    private readonly IUserContext _userContext;

    public GetMedicalHistoriesQueryHandler(
        MedicalAssistantDbContext context,
        IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<IEnumerable<MedicalHistoryDto>> Handle(
        GetMedicalHistoriesQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetUserId;

        var query = _context
            .MedicalHistories
            .AsNoTracking()
            .Where(h => h.UserId == userId)
            .Include(x => x.Visit)
            .Include(x => x.DiseaseStages)
            .ThenInclude(x => x.Visit)
            .Select(x => new MedicalHistoryDto
            {
                Id = x.Id,
                DiseaseName = x.DiseaseName,
                Notes = x.Notes,
                SymptomDescription = x.SymptomDescription,
                StartDate = (DateTime)x.DiseaseStartDate,
                EndDate = x.DiseaseEndDate != null ? (DateTime)x.DiseaseEndDate : null,
                VisitDto = x.Visit != null ? x.Visit.ToDto() : null,
                Stages = x.DiseaseStages.OrderBy(d => d.Date).Select(y => new DiseaseStageDto()
                {
                    Id = y.Id,
                    MedicalHistoryId = y.MedicalHistoryId,
                    Date = (DateTime)y.Date,
                    Note = y.Note,
                    Name = y.Name,
                    VisitDto = y.Visit != null ? y.Visit.ToDto() : null
                }).OrderBy(y => y.Date).ToList()
            });

        if (request.Ids is not null)
        {
            query = query.Where(h => request.Ids.Contains(h.Id));
        }

        var response = await query
            .OrderBy(x => x.StartDate)
            .ToListAsync(cancellationToken);

        return response;
    }
}