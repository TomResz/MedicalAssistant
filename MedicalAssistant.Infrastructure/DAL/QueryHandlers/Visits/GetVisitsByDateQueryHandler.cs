using Dapper;
using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Visits.Queries;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Infrastructure.DAL.Dapper;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Visits;

internal sealed class GetVisitsByDateQueryHandler
	: IQueryHandler<GetVisitsByDateQuery, IEnumerable<VisitDto>>
{
	private readonly IUserContext _userContext;
	private readonly ISqlConnectionFactory _connectionFactory;
	public GetVisitsByDateQueryHandler(
		IUserContext userContext,
		ISqlConnectionFactory connectionFactory)
	{
		_userContext = userContext;
		_connectionFactory = connectionFactory;
	}

	public async Task<IEnumerable<VisitDto>> Handle(GetVisitsByDateQuery request, CancellationToken cancellationToken)
	{
		var userId = _userContext.GetUserId;
		var startOfDay = new Date(request.Date);

		using var connection = _connectionFactory.Create();

		var response = await connection.QueryAsync<VisitDto,Location,VisitDto>($"""
			SELECT v."Id" as  {nameof(VisitDto.VisitId)},
				v."Date" as {nameof(VisitDto.Date)},
				v."DoctorName" as {nameof(VisitDto.DoctorName)},
				v."VisitType" as {nameof(VisitDto.VisitType)},
				v."PredictedEndDate" as {nameof(VisitDto.EndDate)},
				v."VisitDescription" as {nameof(VisitDto.VisitDescription)},
				v."Address_PostalCode" as {nameof(Location.PostalCode)},
				v."Address_Street" as {nameof(Location.Street)},
				v."Address_City" as {nameof(Location.City)}
				FROM "Visits" as v
				WHERE v."UserId" = @userId AND v."Date"::date = @date
			""",
			map: (visit,address) =>
			{
				visit.Address = address;
				return visit;
			},
				param: new { userId = userId.Value, date = startOfDay.Value },
				splitOn: "PostalCode");

		return response;
	}
}
