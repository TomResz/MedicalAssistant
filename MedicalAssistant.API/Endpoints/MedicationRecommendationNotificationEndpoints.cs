using MediatR;
using MedicalAssistant.Application.MedicationNotifications.Commands.Add;
using MedicalAssistant.Application.MedicationNotifications.Queries;
using Microsoft.AspNetCore.Mvc;

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

		group.MapGet("week", async (IMediator _mediator,
			[FromQuery]int offset, [FromQuery] DateTime date) =>
		{
			var query = new GetMedicationNotificationByWeekQuery(offset,date);
			var response = await _mediator.Send(query);
			return Results.Ok(response);

		});
	}
}
