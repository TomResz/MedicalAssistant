using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Dto;
using MedicalAssist.Application.Pagination;
using MedicalAssist.Application.Visits;
using MedicalAssist.Application.Visits.Queries;
using MedicalAssist.Domain.Abstraction;
using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;
using MedicalAssist.Infrastructure.DAL.QueryHandlers.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssist.Infrastructure.DAL.QueryHandlers.Visits;
internal sealed class GetPageOfCurrentVisitsQueryHandler
	: IRequestHandler<GetPageOfCurrentVisitsQuery,PagedList<VisitDto>>
{
	private readonly IUserContext _userContext;
	private readonly MedicalAssistDbContext _context;
	private readonly IClock _clock;

	public GetPageOfCurrentVisitsQueryHandler(IUserContext userContext, MedicalAssistDbContext context, IClock clock)
	{
		_userContext = userContext;
		_context = context;
		_clock = clock;
	}

	public async Task<PagedList<VisitDto>> Handle(GetPageOfCurrentVisitsQuery request, CancellationToken cancellationToken)
	{
		UserId userId = _userContext.GetUserId;

		Date sevenDaysAhead = _clock.GetCurrentUtc().AddDays(request.DaysAhead);
		Date daysBack = _clock.GetCurrentUtc().AddDays(-request.DaysBack);

		IQueryable<VisitDto> query = _context
			.Visits
			.Where(x => x.UserId == userId &&
			   x.Date >= daysBack &&
			   x.Date <= sevenDaysAhead)
			.Select(x => x.ToDto())
			.AsNoTracking()
			.AsQueryable();

		var pagedList = await PagedListFactory<VisitDto>
			.CreateByQueryAsync(query,request.Page,request.PageSize);
		
		switch(request.Direction)
		{
			case OrderDirection.Descending:
				pagedList.Items = pagedList.Items.OrderByDescending(x => x.Date).ToList();
				break;
			case OrderDirection.Ascending:
				pagedList.Items = pagedList.Items.OrderBy(x => x.Date).ToList();
				break;
		};	

		return pagedList;
	}
}
