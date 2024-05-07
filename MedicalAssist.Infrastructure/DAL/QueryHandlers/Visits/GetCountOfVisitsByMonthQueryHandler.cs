using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Dto;
using MedicalAssist.Application.Visits.Queries;
using MedicalAssist.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssist.Infrastructure.DAL.QueryHandlers.Visits;
internal sealed class GetCountOfVisitsByMonthQueryHandler
    : IRequestHandler<GetCountOfVisitsByMonthQuery, IEnumerable<VisitDateAvailableCountDto>>
{
    private readonly MedicalAssistDbContext _context;
    private readonly IUserContext _userContext;
    public GetCountOfVisitsByMonthQueryHandler(MedicalAssistDbContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<IEnumerable<VisitDateAvailableCountDto>> Handle(GetCountOfVisitsByMonthQuery request, CancellationToken cancellationToken)
    {
        UserId userId = _userContext.GetUserId;

        var visits = await _context
            .Database
            .SqlQuery<VisitDateAvailableCountDto>($"""
                SELECT DATE_TRUNC('day', v."Date") AS "VisitDate", COUNT(*) AS "VisitsCount"
                FROM "Visits" v
                WHERE v."UserId" = {userId.Value} AND EXTRACT(MONTH FROM v."Date")={request.Month}
                GROUP BY DATE_TRUNC('day', v."Date")
                ORDER BY DATE_TRUNC('day', v."Date")
            """)
            .ToListAsync(cancellationToken);
        return visits;
    }
}
