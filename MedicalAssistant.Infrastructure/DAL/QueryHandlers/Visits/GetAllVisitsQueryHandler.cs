using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Application.Visits.Queries;
using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Infrastructure.DAL.QueryHandlers.Extensions;
using Microsoft.EntityFrameworkCore;
namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Visits;
internal sealed class GetAllVisitsQueryHandler
	: IQueryHandler<GetAllVisitsQuery, IEnumerable<VisitDto>>
{
	private readonly MedicalAssistantDbContext _context;
	private readonly IUserContext _userContext;
	public GetAllVisitsQueryHandler(MedicalAssistantDbContext context, IUserContext userContext)
	{
		_context = context;
		_userContext = userContext;
	}

	public async Task<IEnumerable<VisitDto>> Handle(GetAllVisitsQuery request, CancellationToken cancellationToken)
	{
		var userId = _userContext.GetUserId;

		var firstPartOfQuery = _context
			.Visits
			.Where(x => x.UserId == userId);

		IQueryable<Visit> sortedQuery = firstPartOfQuery; 

		if (request.Direction.ToLower() == "asc")
		{
			sortedQuery = sortedQuery.OrderBy(x => x.Date);
		}
		else 
		{
			sortedQuery = sortedQuery.OrderByDescending(x => x.Date);
		}

		var visits = await sortedQuery
			.Select(x => x.ToDto())
			.AsNoTracking()
			.ToListAsync(cancellationToken);

		return visits;
	}
}
