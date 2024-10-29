using Dapper;
using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.MedicationRecommendations.Queries;
using MedicalAssistant.Infrastructure.DAL.Dapper;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Recommendation;
internal class GetMedicationRecommendationByWeekQueryHandler
	: IRequestHandler<GetMedicationRecommendationByWeekQuery, IEnumerable<MedicationRecommendationWithDayDto>>
{
	private readonly IUserContext _userContext;
	private readonly ISqlConnectionFactory _connectionFactory;
	public GetMedicationRecommendationByWeekQueryHandler(
		IUserContext userContext,
		ISqlConnectionFactory connectionFactory)
	{
		_userContext = userContext;
		_connectionFactory = connectionFactory;
	}

	public async Task<IEnumerable<MedicationRecommendationWithDayDto>> Handle(GetMedicationRecommendationByWeekQuery request, CancellationToken cancellationToken)
	{
		const string sql = $"""
			WITH week_dates AS (
			    SELECT 
			        generate_series(
			            date_trunc('week', @CurrentDate)::date,
			            date_trunc('week', @CurrentDate)::date + interval '6 days',
			            interval '1 day'
			        )::date AS Day
			)
			SELECT
				wd.Day as {nameof(MedicationRecommendationWithDayDto.Day)},

				n."Id" as {nameof(MedicationRecommendationWithDayDto.Id)},
				n."StartDate" as {nameof(MedicationRecommendationWithDayDto.StartDate)},
				n."EndDate" as {nameof(MedicationRecommendationWithDayDto.EndDate)},
				n."Medicine_TimeOfDay" as {nameof(MedicationRecommendationWithDayDto.TimeOfDay)},
				n."Medicine_Quantity" as {nameof(MedicationRecommendationWithDayDto.Quantity)},
				n."Medicine_Name" as {nameof(MedicationRecommendationWithDayDto.Name)},
				n."ExtraNote" as {nameof(MedicationRecommendationWithDayDto.ExtraNote)},
			
				v."Id" as  {nameof(MedicationRecommendationWithDayDto.Visit.VisitId)},
				v."Date" as {nameof(MedicationRecommendationWithDayDto.Visit.Date)},
				v."DoctorName" as {nameof(MedicationRecommendationWithDayDto.Visit.DoctorName)},
				v."VisitType" as {nameof(MedicationRecommendationWithDayDto.Visit.VisitType)},
				v."PredictedEndDate" as {nameof(MedicationRecommendationWithDayDto.Visit.EndDate)},
				v."VisitDescription" as {nameof(MedicationRecommendationWithDayDto.Visit.VisitDescription)},
				v."Address_PostalCode" as {nameof(Location.PostalCode)},
				v."Address_Street" as {nameof(Location.Street)},
				v."Address_City" as {nameof(Location.City)}
			
			FROM week_dates AS wd
			INNER JOIN 
			    "MedicationRecommendation" n 
			ON 
			    n."StartDate" <= wd.day AND n."EndDate" >= wd.day
			LEFT JOIN
				"Visits" as v
			ON v."Id" = n."VisitId"
			WHERE n."UserId" = @UserId
			ORDER BY wd.Day;
			""";

		using var connection = _connectionFactory.Create();

		var response = await connection.QueryAsync<MedicationRecommendationWithDayDto, VisitDto, Location, MedicationRecommendationWithDayDto>(
			sql: sql,
			map: (dto, visit, location) =>
			{
				if (visit is not null)
				{
					visit.Address = location;
					dto.Visit = visit;
				}
				return dto;
			},
			param: new { CurrentDate = request.Date.Date, UserId = _userContext.GetUserId.Value },
			splitOn: $"{nameof(VisitDto.VisitId)},{nameof(Location.PostalCode)}"
		);

		return response;
	}
}
