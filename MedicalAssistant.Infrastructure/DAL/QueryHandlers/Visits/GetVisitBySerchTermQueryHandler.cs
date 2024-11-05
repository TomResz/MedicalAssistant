using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Application.Visits.Queries;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Visits;
internal sealed class GetVisitBySerchTermQueryHandler
    : IRequestHandler<GetVisitBySerchTermQuery, IEnumerable<VisitDto>>
{
    private readonly MedicalAssistantDbContext _context;
    private readonly IUserContext _userContext;
    public GetVisitBySerchTermQueryHandler(
        MedicalAssistantDbContext context,
        IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<IEnumerable<VisitDto>> Handle(GetVisitBySerchTermQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetUserId;

        var query = _context
            .Visits
            .Where(x => x.UserId == userId &&
                EF.Functions.ToTsVector("english", x.DoctorName + " " + x.VisitType + " " + x.VisitDescription)
                .Matches(EF.Functions.PhraseToTsQuery("english", request.SearchTerm)));

        var sortableQuery = query;

        if(request.Direction == "asc")
        {
            sortableQuery = query
                .OrderBy(x => x.Date);
        }
        else
        {
			sortableQuery = query
	            .OrderByDescending(x => x.Date);
		}


		var response = await sortableQuery
            .Select(x=>x.ToDto())
            .ToListAsync(cancellationToken);

        return response;    
    }
}
