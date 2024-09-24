using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Visits;
using MedicalAssistant.Application.Visits.Queries;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Visits;
internal sealed class GetVisitDetailsQueryHandler
	: IRequestHandler<GetVisitDetailsQuery, VisitDto>
{
	private readonly MedicalAssistDbContext _context;
	private readonly IUserContext _userContext;
	public GetVisitDetailsQueryHandler(
		MedicalAssistDbContext context,
		IUserContext userContext)
	{
		_context = context;
		_userContext = userContext;
	}

	public async Task<VisitDto> Handle(GetVisitDetailsQuery request, CancellationToken cancellationToken)
	{
		var userId = _userContext.GetUserId;
		var visit = await _context
			.Visits
			.AsNoTracking()
			.FirstOrDefaultAsync(
				x => x.UserId == userId &&
					x.Id == new VisitId(request.VisitId), cancellationToken);
		return visit is null ? throw new UnknownVisitException() : visit.ToDto();
	}
}
