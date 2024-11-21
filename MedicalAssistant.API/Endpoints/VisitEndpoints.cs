
using MediatR;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Visits.Commands.AddVisit;
using MedicalAssistant.Application.Visits.Commands.DeleteVisit;
using MedicalAssistant.Application.Visits.Commands.EditVisit;
using MedicalAssistant.Application.Visits.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAssistant.API.Endpoints;

public sealed class VisitEndpoints : IEndpoints
{
	public void MapEndpoints(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("visit")
			.RequireAuthorization(Permissions.Permissions.IsVerifiedAndActive)
			.WithTags("Visits");

		group.MapGet("/", async (IMediator _mediator, [FromQuery]string direction = "asc")
			=> await _mediator.Send(new GetAllVisitsQuery(direction)));

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

		group.MapGet("/{visitId:guid}", async (
			IMediator _mediator, Guid visitId) =>
		{
			var query = new GetVisitDetailsQuery(visitId);
			return Results.Ok(await _mediator.Send(query));
		});

		group.MapDelete("delete/{visitId:guid}", async (
			IMediator _mediator,
			Guid visitId) =>
		{
			var command = new DeleteVisitCommand(visitId);
			await _mediator.Send(command);
			return Results.NoContent();
		});

		group.MapGet("{date:datetime}", async (IMediator _mediator, DateTime date) =>
		{
			var query = new GetVisitsByDateQuery(date.Date);
			var response = await _mediator.Send(query);
			return Results.Ok(response);
		});

		group.MapGet("week/{date:datetime}", async (IMediator _mediator, DateTime date)
			=>
		{
			var query = new GetByWeekVisitQuery(date);
			var response = await _mediator.Send(query);
			return Results.Ok(response);
		});

		group.MapGet("completed/{date:datetime}", async (IMediator _mediator, DateTime date) =>
		{
			var query = new GetCompletedVisitsQuery(date);
			var response = await _mediator.Send(query);
			return Results.Ok(response);
		}).Produces(StatusCodes.Status200OK,typeof(IEnumerable<VisitDto>));


		group.MapGet("search", async (IMediator _mediator,[FromQuery] string searchTerm,[FromQuery]string direction = "asc") =>
		{
			var query = new GetVisitBySerchTermQuery(searchTerm,direction);
			var response = await _mediator.Send(query);
			return Results.Ok(response);	
		});

	}
}
