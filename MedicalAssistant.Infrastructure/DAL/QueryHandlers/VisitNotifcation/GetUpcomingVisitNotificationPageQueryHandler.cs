using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Pagination;
using MedicalAssistant.Application.VisitNotifications.Queries;
using MedicalAssistant.Domain.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.VisitNotifcation;
internal sealed class GetUpcomingVisitNotificationPageQueryHandler
	: IQueryHandler<GetUpcomingVisitNotificationPageQuery, PagedList<UpcomingVisitNotificationDto>>
{
	private readonly IUserContext _userContext;
	private readonly IClock _clock;
	private readonly MedicalAssistantDbContext _context;

	public GetUpcomingVisitNotificationPageQueryHandler(
		IUserContext userContext,
		IClock clock,
		MedicalAssistantDbContext context)
	{
		_userContext = userContext;
		_clock = clock;
		_context = context;
	}

	public async Task<PagedList<UpcomingVisitNotificationDto>> Handle(GetUpcomingVisitNotificationPageQuery request, CancellationToken cancellationToken)
	{
		var userId = _userContext.GetUserId;
		var currentDate = _clock.GetCurrentUtc();

		var query = _context
			.Database 
			.SqlQuery<UpcomingVisitNotificationDto>($"""
				SELECT 
				    vn."Id" AS "Id",
				    v."Id" AS "VisitId",
				    vn."ScheduledDateUtc" AS "ScheduledDateUtc",
				    v."Date" AS "Date",
				    v."DoctorName" AS "DoctorName",
				    v."VisitType" AS "VisitType"
				FROM "VisitNotifications" AS vn
				INNER JOIN "Visits" AS v ON v."Id" = vn."VisitId"
				WHERE v."UserId" = {userId.Value} AND vn."ScheduledDateUtc" >= {currentDate}
				ORDER BY vn."ScheduledDateUtc" ASC 
			""");


		var pagedList = await PagedListFactory<UpcomingVisitNotificationDto>
			.CreateByQueryAsync(query, request.Page, request.PageSize); 
		return pagedList;
	}
}
