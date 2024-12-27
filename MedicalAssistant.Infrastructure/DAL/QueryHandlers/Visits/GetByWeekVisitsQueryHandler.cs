using Dapper;
using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Visits.Queries;
using MedicalAssistant.Infrastructure.DAL.Dapper;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Visits;
internal sealed class GetByWeekVisitsQueryHandler
	: IQueryHandler<GetByWeekVisitQuery, IEnumerable<VisitDto>>
{

	private readonly ISqlConnectionFactory _sqlConnectionFactory;
	private readonly IUserContext _userContext;

	public GetByWeekVisitsQueryHandler(
		IUserContext userContext,
		ISqlConnectionFactory sqlConnectionFactory)
	{
		_userContext = userContext;
		_sqlConnectionFactory = sqlConnectionFactory;
	}

	public async Task<IEnumerable<VisitDto>> Handle(GetByWeekVisitQuery request, CancellationToken cancellationToken)
	{
		var userId = _userContext.GetUserId;

		var date = request.Date.Date;

		using var connection = _sqlConnectionFactory.Create();

		const string sql = $"""
			SELECT 
				v."Id" as  {nameof(VisitDto.VisitId)},
				v."Date" as {nameof(VisitDto.Date)},
				v."DoctorName" as {nameof(VisitDto.DoctorName)},
				v."VisitType" as {nameof(VisitDto.VisitType)},
				v."PredictedEndDate" as {nameof(VisitDto.EndDate)},
				v."VisitDescription" as {nameof(VisitDto.VisitDescription)},
				v."Address_PostalCode" as {nameof(Location.PostalCode)},
				v."Address_Street" as {nameof(Location.Street)},
				v."Address_City" as {nameof(Location.City)}
				FROM "Visits" as v
				WHERE v."UserId" = @userId 
					AND v."Date" >= date_trunc('week', @date)
					AND v."Date" < date_trunc('week', @date) + interval '1 week' 
			""";


		var response = await connection.QueryAsync<VisitDto, Location, VisitDto>(sql: sql,
			map: (visit, address) =>
			{
				visit.Address = address;
				return visit;
			},
			param: new { userId = userId.Value, date = date },
			splitOn: nameof(Location.PostalCode));

		return response;
	}
}
