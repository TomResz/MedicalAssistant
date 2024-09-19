using MediatR;
using MedicalAssistant.Application.VisitNotifications.Commands.AddNotifications;
using MedicalAssistant.Application.VisitNotifications.Commands.DeleteNotification;
using MedicalAssistant.Application.VisitNotifications.Queries;

namespace MedicalAssistant.API.Endpoints;

public class VisitNotificationEndpoints : IEndpoints
{
	public void MapEndpoints(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("visitNotification")
			.WithTags("VisitNotifications")
			.RequireAuthorization(Permissions.Permissions.VerifiedUser);

		group.MapPost("/", async (
			IMediator _mediator,
			AddVisitNotificationCommand command) =>
		{
			var response = await _mediator.Send(command);
			return Results.Ok(response);
		});

		group.MapGet("/visit={visitId:guid}", async (
			IMediator _mediator, Guid visitId) =>
		{
			var query = new GetNotificationByVisitQuery(visitId);
			var response = await _mediator.Send(query);
			return Results.Ok(response);
		});

		group.MapDelete("/{visitNoticationId:guid}", async (
			IMediator _mediator, Guid visitNoticationId) =>
		{
			var command = new DeleteVisitNotificationCommand(visitNoticationId);
			await _mediator.Send(command);
			return Results.NoContent();
		});
	}
}
