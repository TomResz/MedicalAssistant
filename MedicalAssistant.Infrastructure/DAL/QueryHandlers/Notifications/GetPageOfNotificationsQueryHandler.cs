using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Notifications.Queries;
using MedicalAssistant.Application.Pagination;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Notifications;

internal sealed class GetPageOfNotificationsQueryHandler
	: IRequestHandler<GetPageOfNotificationsQuery, PagedList<NotificationDto>>
{
	private readonly MedicalAssistantDbContext _context;
	private readonly IUserContext _userContext;

	public GetPageOfNotificationsQueryHandler(
		MedicalAssistantDbContext context,
		IUserContext userContext)
	{
		_context = context;
		_userContext = userContext;
	}

	public async Task<PagedList<NotificationDto>> Handle(GetPageOfNotificationsQuery request, CancellationToken cancellationToken)
	{
		var userId = _userContext.GetUserId;
		var query = _context
			.NotificationHistories
			.Where(x => x.UserId == userId)
			.OrderByDescending(x=>x.PublishedDate)
			.Select(x => new NotificationDto()
			{
				Id=x.Id,	
				ContentJson = x.ContentJson,
				Type = x.Type,
				WasRead = x.WasRead,
				PublishedDateUtc = x.PublishedDate.ToDate()
			});

		PagedList<NotificationDto> pagedHistoryList = await PagedListFactory<NotificationDto>.CreateByQueryAsync(
			query,
			request.Page,
			request.PageSize);

		return pagedHistoryList;
	}
}
