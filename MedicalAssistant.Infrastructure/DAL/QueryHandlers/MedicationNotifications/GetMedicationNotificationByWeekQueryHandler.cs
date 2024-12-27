using Dapper;
using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.MedicationNotifications.Queries;
using MedicalAssistant.Infrastructure.DAL.Dapper;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.MedicationNotifications;
internal sealed class GetMedicationNotificationByWeekQueryHandler
	: IQueryHandler<GetMedicationNotificationByWeekQuery, IEnumerable<MedicationNotificationDto>>
{
	private readonly IUserContext _userContext;
	private readonly ISqlConnectionFactory _connectionFactory;
	public GetMedicationNotificationByWeekQueryHandler(
		IUserContext userContext,
		ISqlConnectionFactory connectionFactory)
	{
		_userContext = userContext;
		_connectionFactory = connectionFactory;
	}

	public async Task<IEnumerable<MedicationNotificationDto>> Handle(GetMedicationNotificationByWeekQuery request, CancellationToken cancellationToken)
	{
		var userId = _userContext.GetUserId;

		const string query = $"""
			WITH week_dates AS (
			    SELECT 
			        generate_series(
			            date_trunc('week', @CurrentDate)::date,
			            date_trunc('week', @CurrentDate)::date + interval '6 days',
			            interval '1 day'
			        )::date AS Day
			)
			SELECT 
			    wd.Day as {nameof(MedicationNotificationDto.Day)},
			    n."Id" as {nameof(MedicationNotificationDto.Id)},
				n."TriggerTimeUtc" as {nameof(MedicationNotificationDto.Time)},
				med."Medicine_Name" as {nameof(MedicationNotificationDto.MedicineName)} ,
				med."Medicine_TimeOfDay" as {nameof(MedicationNotificationDto.TimesOfDay)},
				med."Id" as {nameof(MedicationNotificationDto.MedicationId)}
			FROM 
			    week_dates AS wd
			LEFT JOIN 
			    "MedicationRecommendationsNotifications" n 
			ON 
			    n."Start" <= wd.day AND n."End" >= wd.day

			INNER JOIN 
				"MedicationRecommendation" as med
			ON
				med."Id" = n."MedicationRecommendationId"
			WHERE med."UserId"= @UserId
			ORDER BY wd.Day;
			""";

		using var connection = _connectionFactory.Create();

		var response = await connection.QueryAsync<MedicationNotificationDto>(
			sql: query,
			param: new
			{
				CurrentDate = request.Date.Date,
				UserId = _userContext.GetUserId.Value
			});

		foreach (var item in response)
		{
			item.Time = item.Time.AddHours(request.Offset);
		}

		return response;
	}
}