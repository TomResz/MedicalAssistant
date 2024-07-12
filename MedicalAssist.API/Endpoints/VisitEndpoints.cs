
using MediatR;
using MedicalAssist.API.QueryPolicy;
using MedicalAssist.Application.Visits.Commands.AddVisit;
using MedicalAssist.Application.Visits.Queries;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MedicalAssist.API.Endpoints;

public sealed class VisitEndpoints : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("visit")
			.RequireAuthorization()
			.WithTags("Visits");

		group.MapPost("add", async (IMediator _mediator, AddVisitCommand command) =>
		{
			await _mediator.Send(command);
			return Results.Created("api/Visit/add", null);
		});

		group.MapGet("current", async (
			IMediator _mediator,
			[AsParameters] PageParameters pageParameters,
			[FromQuery] string direction = "asc") =>
		{
			var order = SortingParameters.FromString(direction);
			var query = new GetPageOfCurrentVisitsQuery(pageParameters.Page, pageParameters.PageSize, order);
			return Results.Ok(await _mediator.Send(query));
		});

		group.MapGet("{id:guid}", async (IMediator _mediator, Guid id) =>
			Results.Ok(await _mediator.Send(new GetDetailsOfVisitQuery(id))));

		group.MapGet("calendar", async (IMediator _mediator)
			=> Results.Ok(await _mediator.Send(new GetCountOfVisitsQuery())));

		group.MapGet("month={month:int}", async (IMediator _mediator, [FromRoute][Range(1, 12)] int month) =>
			Results.Ok(await _mediator.Send(new GetCountOfVisitsByMonthQuery(month))));

		group.MapGet("day={day}", async (IMediator _mediator, DateTime day) 
			=> Results.Ok(await _mediator.Send(new GetVisitsByDayQuery(day))));

	}
}
