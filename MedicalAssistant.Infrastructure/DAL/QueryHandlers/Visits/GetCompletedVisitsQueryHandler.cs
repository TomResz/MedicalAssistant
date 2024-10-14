using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Application.Visits.Queries;
using MedicalAssistant.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Visits;

internal sealed class GetCompletedVisitsQueryHandler
	: IRequestHandler<GetCompletedVisitsQuery, IEnumerable<VisitDto>>
{
	private readonly IUserContext _userContext;
	private readonly MedicalAssistantDbContext _context;
	public GetCompletedVisitsQueryHandler(
		IUserContext userContext,
		MedicalAssistantDbContext context)
	{
		_userContext = userContext;
		_context = context;
	}

	public async Task<IEnumerable<VisitDto>> Handle(GetCompletedVisitsQuery request, CancellationToken cancellationToken)
	{
		var userId = _userContext.GetUserId;
		Date date = request.CurrentDate;

		var response = await _context
			.Visits
			.Where(x=>x.UserId == userId && x.Date < date)
			.AsNoTracking()
			.OrderByDescending(x=>x.Date)
			.Select(x=>x.ToDto())
			.ToListAsync(cancellationToken);

		return response;
	}
}
