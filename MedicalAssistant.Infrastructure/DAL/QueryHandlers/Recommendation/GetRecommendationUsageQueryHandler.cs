using Dapper;
using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.MedicationRecommendations.Queries;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Infrastructure.DAL.Dapper;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Recommendation;
internal sealed class GetRecommendationUsageQueryHandler : IRequestHandler<GetRecommendationUsageQuery,IEnumerable<RecommendationUsageDto>>
{
	private readonly ISqlConnectionFactory _connectionFactory;
	private readonly IUserContext _userContext;
	public GetRecommendationUsageQueryHandler(ISqlConnectionFactory connectionFactory, IUserContext userContext)
	{
		_connectionFactory = connectionFactory;
		_userContext = userContext;
	}

	public async Task<IEnumerable<RecommendationUsageDto>> Handle(GetRecommendationUsageQuery request, CancellationToken cancellationToken)
	{
		var userId = _userContext.GetUserId.Value;
		var date = new Date(request.Date.Date);

		const string sql = """
			   SELECT 
			    mr."Id" AS RecommendationId,
				mr."Medicine_Name" as Name,
			    p.time_of_day AS TimeOfDay,
			    @Date as Date,
				ru."Id" as Id,
			    CASE 
			        WHEN ru."MedicationRecommendationId" IS NOT NULL THEN True
			        ELSE False
			    END AS Status
			FROM 
			    "MedicationRecommendation" mr 
			JOIN 
			    LATERAL unnest(mr."Medicine_TimeOfDay") AS p(time_of_day) ON TRUE
			LEFT JOIN 
			    "RecommendationUsages" ru 
			ON 
			    ru."MedicationRecommendationId" = mr."Id"
			    AND ru."Date" = @Date
			    AND ru."TimeOfDay" = p.time_of_day
			WHERE 
			    mr."StartDate" <= @Date 
			    AND mr."EndDate" >= @Date
				AND mr."UserId" = @UserId
			""";

		using var connection = _connectionFactory.Create();

		var result = await connection.QueryAsync<RecommendationUsageDto>(sql,
			new {
				UserId = userId,
				Date = date.Value
			});

		return result;
	}
}
