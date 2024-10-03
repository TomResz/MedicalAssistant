using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Visits.Queries;
using MedicalAssistant.Infrastructure.DAL.QueryHandlers.Extensions;
using Microsoft.EntityFrameworkCore;
using  MedicalAssistant.Application.Visits;
namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Visits;
internal sealed class GetAllVisitsQueryHandler
	: IRequestHandler<GetAllVisitsQuery, IEnumerable<VisitDto>>
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

		var visits = await _context
			.Visits
			.Where(x => x.UserId == userId)
			.Select(x => x.ToDto())
			.AsNoTracking()
			.ToListAsync(cancellationToken);
		return visits;
	}
}
