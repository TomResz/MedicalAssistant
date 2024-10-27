using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Notifications.Queries;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Notifications;
internal sealed class GetNotificationsQueryHandler
	: IRequestHandler<GetNotificationsQuery, IEnumerable<NotificationDto>>
{
	private MedicalAssistantDbContext _context;
	private readonly IUserContext _userContext;
	public GetNotificationsQueryHandler(
		MedicalAssistantDbContext context,
		IUserContext userContext)
	{
		_context = context;
		_userContext = userContext;
	}

	public async Task<IEnumerable<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
	{
		var userId = _userContext.GetUserId;

		var notifications = await _context.NotificationHistories
			.Where(x => x.UserId == userId)
			.OrderByDescending(x => x.PublishedDate)
			.Select(x => new NotificationDto
			{
				Id = x.Id,
				WasRead = x.WasRead,
				ContentJson = x.ContentJson,
				PublishedDateUtc = (DateTime)x.PublishedDate,
				Type = x.Type
			})
			.ToListAsync(cancellationToken);

		var unreadNotifications = notifications
			.Where(x => !x.WasRead)
			.OrderByDescending(x=>x.PublishedDateUtc)
			.ToList();

		var readNotifications = notifications
			.Where(x => x.WasRead)
			.OrderByDescending(x=>x.PublishedDateUtc)
			.Take(15)
			.ToList();

		return [.. unreadNotifications,.. readNotifications];
	}
}
