using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Application.Visits.Queries;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Visits;
internal sealed class GetVisitBySearchTermQueryHandler
    : IRequestHandler<GetVisitBySerchTermQuery, IEnumerable<VisitDto>>
{
    private readonly MedicalAssistantDbContext _context;
    private readonly IUserContext _userContext;
    public GetVisitBySearchTermQueryHandler(
        MedicalAssistantDbContext context,
        IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<IEnumerable<VisitDto>> Handle(GetVisitBySerchTermQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetUserId;

        if (string.IsNullOrEmpty(request.SearchTerm))
        {
            return Enumerable.Empty<VisitDto>();
        }
        
        var formattedQuery = request.SearchTerm.ToTsQuery();
        
        var query = _context
            .Visits
            .Where(x => x.UserId == userId &&
                EF.Functions.ToTsVector("english", x.DoctorName + " " + x.VisitType + " " + x.VisitDescription)
                .Matches(EF.Functions.ToTsQuery("english", formattedQuery)));

        if(request.Direction == "asc")
        {
            query = query
                .OrderBy(x => x.Date);
        }
        else
        {
			query = query
	            .OrderByDescending(x => x.Date);
		}


		var response = await query
            .Select(x=>x.ToDto())
            .ToListAsync(cancellationToken);

        return response;    
    }
}
