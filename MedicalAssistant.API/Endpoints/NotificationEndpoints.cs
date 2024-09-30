using MediatR;
using MedicalAssistant.Application.Notifications.Commands.MarkAsRead;
using MedicalAssistant.Application.Notifications.Queries;

namespace MedicalAssistant.API.Endpoints;

public class NotificationEndpoints : IEndpoints
{
	public void MapEndpoints(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("notification")
			.WithTags("Notifications")
			.RequireAuthorization(Permissions.Permissions.VerifiedUser);

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
	}
}
