using MediatR;
using MedicalAssistant.Application.MedicationRecommendations.Commands.DeleteRecommendation;
using MedicalAssistant.Application.Notifications.Commands.MarkAsRead;
using MedicalAssistant.Application.Notifications.Queries;

namespace MedicalAssistant.API.Endpoints;

public class NotificationEndpoints : IEndpoints
{
	public void MapEndpoints(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("notification")
			.WithTags("Notifications")
			.RequireAuthorization(Permissions.Permissions.IsVerifiedAndActive);

		group.MapGet("unread", async (IMediator _mediator) =>
		{
			var response = await _mediator.Send(new GetUnreadNotificationsQuery());
			return Results.Ok(response);
		});

		group.MapPost("mark-as-read", async (IMediator _mediator, MarkAsReadNotificationCommand command) =>
		{
			await _mediator.Send(command);
			return Results.NoContent();
		});

		group.MapGet("/", async (IMediator _mediator,int Page,int PageSize) =>
		{	
			var query = new GetPageOfNotificationsQuery(Page, PageSize);
			var response = await _mediator.Send(query);
			return Results.Ok(response);
		});

		group.MapGet("all", async (IMediator _mediator) =>
		{
			var query = new GetNotificationsQuery();
			var response = await _mediator.Send(query);
			return Results.Ok(response);	
		});

		group.MapDelete("/{id:guid}", async (IMediator _mediator,Guid id) =>
		{
			var command = new DeleteRecommendationCommand(id);
			await _mediator.Send(command);
			return Results.NoContent();
		});
	}
}
