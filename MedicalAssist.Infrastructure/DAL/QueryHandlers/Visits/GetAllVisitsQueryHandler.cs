using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Dto;
using MedicalAssist.Application.Visits.Queries;
using MedicalAssist.Infrastructure.DAL.QueryHandlers.Extensions;
using Microsoft.EntityFrameworkCore;
using  MedicalAssist.Application.Visits;
namespace MedicalAssist.Infrastructure.DAL.QueryHandlers.Visits;
internal sealed class GetAllVisitsQueryHandler
	: IRequestHandler<GetAllVisitsQuery, IEnumerable<VisitDto>>
{
	private readonly MedicalAssistDbContext _context;
	private readonly IUserContext _userContext;
	public GetAllVisitsQueryHandler(MedicalAssistDbContext context, IUserContext userContext)
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
