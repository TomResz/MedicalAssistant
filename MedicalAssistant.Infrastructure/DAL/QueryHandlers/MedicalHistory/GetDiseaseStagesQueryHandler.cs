using MediatR;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Application.MedicalHistory.Query;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.MedicalHistory;

internal sealed class GetDiseaseStagesQueryHandler
    : IRequestHandler<GetDiseaseStagesQuery, IEnumerable<DiseaseStageDto>>
{
    private readonly MedicalAssistantDbContext _context;

    public GetDiseaseStagesQueryHandler(
        MedicalAssistantDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DiseaseStageDto>> Handle(
        GetDiseaseStagesQuery request, CancellationToken cancellationToken)
    {
        var id = new MedicalHistoryId(request.MedicalHistoryId);
        
        var response = await _context
            .DiseaseStages
            .Where(x => x.MedicalHistoryId == id)
            .Include(x=>x.Visit)
            .Select(y => new DiseaseStageDto()
            {
                Id = y.Id,
                MedicalHistoryId = y.MedicalHistoryId,
                Date = (DateTime)y.Date,
                Note = y.Note,
                Name = y.Name,
                VisitDto = y.Visit != null ? y.Visit.ToDto() : null
            })
            .ToListAsync(cancellationToken);

        return response;
    }
}