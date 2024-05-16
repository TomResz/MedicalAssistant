using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Dto;
using MedicalAssist.Application.Visits.Queries;
using MedicalAssist.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssist.Infrastructure.DAL.QueryHandlers.Visits;
internal class GetCountOfVisitsQueryHandler
    : IRequestHandler<GetCountOfVisitsQuery, IEnumerable<VisitDateAvailableCountDto>>
{
    private readonly IUserContext _userContext;
    private readonly MedicalAssistDbContext _context;
    public GetCountOfVisitsQueryHandler(IUserContext userContext, MedicalAssistDbContext context)
    {
        _userContext = userContext;
        _context = context;
    }

    public async Task<IEnumerable<VisitDateAvailableCountDto>> Handle(GetCountOfVisitsQuery request, CancellationToken cancellationToken)
    {
        UserId userId = _userContext.GetUserId;

        var visits = await _context
            .Database
            .SqlQuery<VisitDateAvailableCountDto>(
            $@"
                SELECT DATE_TRUNC('day', v.""Date"") AS ""VisitDate"", COUNT(*) AS ""VisitsCount""
                FROM ""Visits"" v
                WHERE v.""UserId"" = {userId.Value}
                GROUP BY DATE_TRUNC('day', v.""Date"")
                ORDER BY DATE_TRUNC('day', v.""Date"")"
             )
            .ToListAsync(cancellationToken);
        return visits;  
    }
}
