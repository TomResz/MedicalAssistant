using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Dto;
using MedicalAssist.Application.Visits.Queries;
using MedicalAssist.Domain.ValueObjects.IDs;
using MedicalAssist.Infrastructure.DAL.QueryHandlers.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssist.Infrastructure.DAL.QueryHandlers.Visits;
internal sealed class GetVisitsByDayQueryHandler : IRequestHandler<GetVisitsByDayQuery, IEnumerable<VisitDto>>
{
    private readonly IUserContext _userContext;
    private readonly MedicalAssistDbContext _context;
    public GetVisitsByDayQueryHandler(IUserContext userContext, MedicalAssistDbContext context)
    {
        _userContext = userContext;
        _context = context;
    }

    public async Task<IEnumerable<VisitDto>> Handle(GetVisitsByDayQuery request, CancellationToken cancellationToken)
    {
        UserId userId = _userContext.GetUserId;

        var visits = await _context
            .Visits
            .FromSql($"""
            SELECT *
            FROM "Visits" v
            Where Date(v."Date")=Date({request.Day.Date}) AND v."UserId"={userId.Value}
            """)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);
        return visits;
    }
}
