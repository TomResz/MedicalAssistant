using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.MedicationNotifications.Queries;
using MedicalAssistant.Application.Pagination;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.MedicationNotifications;

internal sealed class GetUpcomingMedicationNotificationPageQueryHandler
	: IRequestHandler<GetUpcomingMedicationNotificationPageQuery, PagedList<MedicationNotificationPageContentDto>>
{
	private readonly IUserContext _userContext;
	private readonly MedicalAssistantDbContext _context;
	public GetUpcomingMedicationNotificationPageQueryHandler(
		IUserContext userContext,
		MedicalAssistantDbContext context)
	{
		_userContext = userContext;
		_context = context;
	}

	public async Task<PagedList<MedicationNotificationPageContentDto>> Handle(GetUpcomingMedicationNotificationPageQuery request, CancellationToken cancellationToken)
	{
		var userId = _userContext.GetUserId;

		var query = _context
			.Database
			.SqlQuery<MedicationNotificationPageContentDto>($"""
			SELECT 
				n."Id" as "Id",
				n."Start" as "StartDate",
				n."End" as "EndDate",
				n."TriggerTimeUtc" + ({request.Offset} || ' hours')::INTERVAL as "Time", 
				m."Medicine_Name" as "MedicineName",
				m."Id" as "MedicationId"
			FROM "MedicationRecommendationsNotifications" as n
			INNER JOIN "MedicationRecommendation" as m 
				on m."Id" = n."MedicationRecommendationId"
			WHERE
			 {request.Date.Date} BETWEEN n."Start" AND n."End"  AND
				m."UserId" = {userId.Value}
			""");

		var pagedList = await PagedListFactory<MedicationNotificationPageContentDto>
				.CreateByQueryAsync(query, request.Page, request.PageSize);
		return pagedList;
	}
}
