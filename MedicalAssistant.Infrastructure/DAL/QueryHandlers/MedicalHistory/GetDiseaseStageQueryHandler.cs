using MediatR;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.MedicalHistory.Query;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.MedicalHistory;

internal sealed class GetDiseaseStageQueryHandler
    : IRequestHandler<GetDiseaseStageQuery, DiseaseStageDto>
{
    private readonly MedicalAssistantDbContext _context;

    public GetDiseaseStageQueryHandler(MedicalAssistantDbContext context)
    {
        _context = context;
    }

    public async Task<DiseaseStageDto> Handle(GetDiseaseStageQuery request, CancellationToken cancellationToken)
    {
        var response = await _context
            .DiseaseStages
            .Include(x => x.Visit)
            .Where(x => x.Id == new DiseaseStageId(request.Id))
            .Select(y => new DiseaseStageDto()
            {
                Id = y.Id,
                MedicalHistoryId = y.MedicalHistoryId,
                Date = (DateTime)y.Date,
                Note = y.Note,
                Name = y.Name,
                VisitDto = y.Visit != null ? y.Visit.ToDto() : null
            })
            .FirstOrDefaultAsync(cancellationToken);
        
        return response ?? throw new UnknownDiseaseStageException(request.Id);
    }
}