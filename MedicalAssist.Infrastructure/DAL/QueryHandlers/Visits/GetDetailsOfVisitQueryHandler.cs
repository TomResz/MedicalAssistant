using MediatR;
using MedicalAssist.Application.Dto;
using MedicalAssist.Application.Visits.Queries;
using MedicalAssist.Domain.Exceptions;
using MedicalAssist.Domain.ValueObjects.IDs;
using MedicalAssist.Infrastructure.DAL.QueryHandlers.Extensions;
using Microsoft.EntityFrameworkCore;
using MedicalAssist.Application.Visits;

namespace MedicalAssist.Infrastructure.DAL.QueryHandlers.Visits;
internal sealed class GetDetailsOfVisitQueryHandler : IRequestHandler<GetDetailsOfVisitQuery, VisitDto>
{
    private readonly MedicalAssistDbContext _context;

    public GetDetailsOfVisitQueryHandler(MedicalAssistDbContext context)
    {
        _context = context;
    }

    public async Task<VisitDto> Handle(GetDetailsOfVisitQuery request, CancellationToken cancellationToken) 
        => await _context
            .Visits
            .AsNoTracking()
            .Where(x => x.Id == new VisitId(request.VisitId))
            .Select(x => x.ToDto())
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new UnknownVisitException(request.VisitId);
}
