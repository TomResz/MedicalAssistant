using MediatR;
using MedicalAssistant.Application.MedicationNotifications.Commands.Add;

namespace MedicalAssistant.API.Endpoints;

public class MedicationRecommendationNotificationEndpoints
	: IEndpoints
{
	public void MapEndpoints(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("recommendationNotification")
			.WithTags("Medication Recommendation Notifications")
			.RequireAuthorization(Permissions.Permissions.VerifiedUser);

		group.MapPost("/", async (IMediator _mediator, AddMedicationNotificationCommand command) =>
		{
			await _mediator.Send(command);
			return Results.NoContent();
		});
	}
}
