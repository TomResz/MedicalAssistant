using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Visits;
using MedicalAssistant.Application.Visits.Queries;
using MedicalAssistant.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Visits;
internal sealed class GetByWeekVisitsQueryHandler
	: IRequestHandler<GetByWeekVisitQuery, IEnumerable<VisitDto>>
{
	private readonly MedicalAssistantDbContext _context;
	private readonly IUserContext _userContext;

	public GetByWeekVisitsQueryHandler(
		MedicalAssistantDbContext context,
		IUserContext userContext)
	{
		_context = context;
		_userContext = userContext;
	}

	public async Task<IEnumerable<VisitDto>> Handle(GetByWeekVisitQuery request, CancellationToken cancellationToken)
	{
		var userId = _userContext.GetUserId;

		var startOfWeek = request.Date.Date;

		if (startOfWeek.DayOfWeek != DayOfWeek.Monday)
		{
			startOfWeek = startOfWeek.AddDays(-(int)startOfWeek.DayOfWeek + (int)DayOfWeek.Monday);
		}
		var endOfWeek = startOfWeek.AddDays(7).AddSeconds(-1);

		var startDate = new Date(startOfWeek);
		var endDate = new Date(endOfWeek);


		var visits = await _context
			.Visits
			.Where(x => x.Date >= startDate 
				&& x.Date <= endDate 
				&& x.UserId == userId)
			.AsNoTracking()
			.OrderBy(x => x.Date)
			.Select(x => x.ToDto())
			.ToListAsync(cancellationToken);

		return visits;
	}
}
