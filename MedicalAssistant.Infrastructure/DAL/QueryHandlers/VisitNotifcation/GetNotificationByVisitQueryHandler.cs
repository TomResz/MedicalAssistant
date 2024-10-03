using MediatR;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Application.VisitNotifications.Queries;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.VisitNotifcation;
internal sealed class GetNotificationByVisitQueryHandler
	: IRequestHandler<GetNotificationByVisitQuery, IEnumerable<VisitNotificationDto>>
{
	private readonly MedicalAssistantDbContext _context;

	public GetNotificationByVisitQueryHandler(MedicalAssistantDbContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<VisitNotificationDto>> Handle(GetNotificationByVisitQuery request, CancellationToken cancellationToken)
		=> await _context
			.VisitNotifications
			.Where(x => x.VisitId == new VisitId(request.VisitId))
			.OrderBy(x=>x.ScheduledDateUtc)
			.Select(x => x.ToDto())
			.ToListAsync(cancellationToken);
}
