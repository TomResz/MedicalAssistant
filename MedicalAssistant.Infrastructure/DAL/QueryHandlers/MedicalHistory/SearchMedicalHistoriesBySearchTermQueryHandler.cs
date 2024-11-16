using System.Text.RegularExpressions;
using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Application.MedicalHistory.Query;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.MedicalHistory;

internal sealed class SearchMedicalHistoriesBySearchTermQueryHandler
    : IRequestHandler<SearchMedicalHistoriesBySearchTermQuery, IEnumerable<MedicalHistoryDto>>
{
    private readonly IUserContext _userContext;
    private readonly MedicalAssistantDbContext _context;
    
    public SearchMedicalHistoriesBySearchTermQueryHandler(
        IUserContext userContext,
        MedicalAssistantDbContext context)
    {
        _userContext = userContext;
        _context = context;
    }

    public async Task<IEnumerable<MedicalHistoryDto>> Handle(SearchMedicalHistoriesBySearchTermQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.SearchTerm))
        {
            return Enumerable.Empty<MedicalHistoryDto>();
        }
        var userId = _userContext.GetUserId;

        var formattedQuery = request.SearchTerm.ToTsQuery();
        
        var response = await _context
            .MedicalHistories
            .AsNoTracking()
            .AsNoTracking()
            .Where(h => h.UserId == userId && 
                        EF.Functions.ToTsVector("english", h.DiseaseName + " " + h.SymptomDescription)
                            .Matches(EF.Functions.ToTsQuery("english", formattedQuery)))
            .Include(x => x.Visit)
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
                VisitDto = x.Visit !=null ? x.Visit.ToDto() : null,
                Stages = x.DiseaseStages.Select(y => new DiseaseStageDto()
                {
                    Id = y.Id,
                    MedicalHistoryId = y.MedicalHistoryId,
                    Date = (DateTime)y.Date,
                    Note = y.Note,
                    Name = y.Name,
                    VisitDto = y.Visit != null ? y.Visit.ToDto() : null
                }).OrderByDescending(y=>y.Date)
                .ToList()
            })
            .OrderBy(x=>x.StartDate)
            .ToListAsync(cancellationToken);
        
        return response;
    }
}