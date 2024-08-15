
using MediatR;
using MedicalAssist.API.QueryPolicy;
using MedicalAssist.Application.Visits.Commands.AddVisit;
using MedicalAssist.Application.Visits.Commands.DeleteVisit;
using MedicalAssist.Application.Visits.Commands.EditVisit;
using MedicalAssist.Application.Visits.Queries;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MedicalAssist.API.Endpoints;

public sealed class VisitEndpoints : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("visit")
			.RequireAuthorization(Permissions.Permissions.VerifiedUser)
			.WithTags("Visits");

		group.MapGet("/", async (IMediator _mediator)
			=> await _mediator.Send(new GetAllVisitsQuery()));

		group.MapPost("add", async (
			IMediator _mediator,
			AddVisitCommand command) =>
		{
			var result = await _mediator.Send(command);
			return Results.Created("api/Visit/add", result);
		});

		group.MapPut("edit", async (
			IMediator _mediator,
			EditVisitCommand command) =>
		{
			var response = await _mediator.Send(command);
			return Results.Ok(response);
		});

		group.MapDelete("delete/{visitId:guid}", async (
			IMediator _mediator, 
			Guid visitId) =>
		{
			var command = new DeleteVisitCommand(visitId);
			await _mediator.Send(command);
			return Results.NoContent();
		});
		

		group.MapGet("current", async (
			IMediator _mediator,
			[AsParameters] PageParameters pageParameters,
			[FromQuery] string direction = "asc",
			[FromQuery][Range(1, int.MaxValue)] int daysBack = 1,
			[FromQuery][Range(1, int.MaxValue)] int daysAhead = 7) =>
		{
			var order = SortingParameters.FromString(direction);
			var query = new GetPageOfCurrentVisitsQuery(
				 pageParameters.Page,
					pageParameters.PageSize,
					  order,
						daysAhead,
						daysBack);
			return Results.Ok(await _mediator.Send(query));
		});

		group.MapGet("{id:guid}", async (
			IMediator _mediator,
			Guid id) =>
			Results.Ok(await _mediator.Send(new GetDetailsOfVisitQuery(id))));

		group.MapGet("calendar", async (IMediator _mediator)
			=> Results.Ok(await _mediator.Send(new GetCountOfVisitsQuery())));

		group.MapGet("month={month:int}", async (
			IMediator _mediator,
			[FromRoute][Range(1, 12)] int month)
			=> Results.Ok(await _mediator.Send(new GetCountOfVisitsByMonthQuery(month))));

		group.MapGet("day={day}", async (IMediator _mediator, DateTime day)
			=> Results.Ok(await _mediator.Send(new GetVisitsByDayQuery(day))));

	}
}
