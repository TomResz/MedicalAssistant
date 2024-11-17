using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.MedicalHistory.Query;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.MedicalHistory;

internal sealed class GetMedicalHistoryByIdQueryHandler
    : IRequestHandler<GetMedicalHistoryByIdQuery, MedicalHistoryDto>
{
    private readonly IUserContext _userContext;
    private readonly MedicalAssistantDbContext _context;

    public GetMedicalHistoryByIdQueryHandler(
        IUserContext userContext,
        MedicalAssistantDbContext context)
    {
        _userContext = userContext;
        _context = context;
    }

    public async Task<MedicalHistoryDto> Handle(GetMedicalHistoryByIdQuery request, CancellationToken cancellationToken)
    {
        var id = new MedicalHistoryId(request.Id);
        var userId = _userContext.GetUserId;

        var response = await _context
            .MedicalHistories
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.Id == id)
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
                Stages = x.DiseaseStages.OrderBy(d=>d.Date).Select(y => new DiseaseStageDto()
                {
                    Id = y.Id,
                    MedicalHistoryId = y.MedicalHistoryId,
                    Date = (DateTime)y.Date,
                    Note = y.Note,
                    Name = y.Name,
                    VisitDto = y.Visit != null ? y.Visit.ToDto() : null
                }).ToList()
            }).FirstOrDefaultAsync(cancellationToken);

        return response ?? throw new UnknownMedicalHistoryException(request.Id);
    }
}