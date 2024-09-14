
using MediatR;
using MedicalAssistant.Application.Visits.Commands.AddVisit;
using MedicalAssistant.Application.Visits.Commands.DeleteVisit;
using MedicalAssistant.Application.Visits.Commands.EditVisit;
using MedicalAssistant.Application.Visits.Queries;

namespace MedicalAssistant.API.Endpoints;

public sealed class VisitEndpoints : IEndpoints
{
	public void MapEndpoints(IEndpointRouteBuilder app)
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
	}
}
