using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Application.Visits.Queries;
using MedicalAssistant.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Visits;

internal sealed class GetVisitsByDateQueryHandler
	: IRequestHandler<GetVisitsByDateQuery, IEnumerable<VisitDto>>
{
	private readonly MedicalAssistantDbContext _context;
	private readonly IUserContext _userContext;
	public GetVisitsByDateQueryHandler(
		MedicalAssistantDbContext context,
		IUserContext userContext)
	{
		_context = context;
		_userContext = userContext;
	}

	public async Task<IEnumerable<VisitDto>> Handle(GetVisitsByDateQuery request, CancellationToken cancellationToken)
	{
		var userId = _userContext.GetUserId;
		var startOfDay = new Date(request.Date);
		var endOfDay = new Date(request.Date.AddDays(1).AddTicks(-1));

		var response = await _context
			.Visits
			.Where(x => x.Date >= startOfDay &&
				x.Date <= endOfDay
				&& x.UserId == userId)
			.AsNoTracking()
			.Select(x => x.ToDto())
			.ToListAsync(cancellationToken);

		return response;	
	}
}
