using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Notifications.Queries;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Notifications;
internal sealed class GetUnreadNotificationsQueryHandler
	: IRequestHandler<GetUnreadNotificationsQuery, List<NotificationDto>>
{
	private readonly MedicalAssistantDbContext _context;
	private readonly IUserContext _userContext;
	public GetUnreadNotificationsQueryHandler(
		MedicalAssistantDbContext context,
		IUserContext userContext)
	{
		_context = context;
		_userContext = userContext;
	}

	public async Task<List<NotificationDto>> Handle(GetUnreadNotificationsQuery request, CancellationToken cancellationToken)
	{
		var userId = _userContext.GetUserId;
		var response = await _context
			.NotificationHistories
			.Where(x => x.UserId == userId &&
						 x.WasRead == false)
			.Select(x=> new NotificationDto
			{
				Id = x.Id,
				WasRead = x.WasRead,
				ContentJson = x.ContentJson,
				Type = x.Type
			})
			.AsNoTracking()
			.ToListAsync(cancellationToken);
		return response;

	}
}
