using MediatR;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.MedicationNotifications.Commands.Add;
using MedicalAssistant.Application.MedicationNotifications.Commands.Delete;
using MedicalAssistant.Application.MedicationNotifications.Commands.Edit;
using MedicalAssistant.Application.MedicationNotifications.Queries;
using MedicalAssistant.Application.MedicationRecommendations.Queries;
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
			var id = await _mediator.Send(command);
			return Results.Ok(id);
		});


		group.MapGet("week", async (IMediator _mediator,
			[FromQuery]int offset, [FromQuery] DateTime date) =>
		{
			var query = new GetMedicationNotificationByWeekQuery(offset,date);
			var response = await _mediator.Send(query);
			return Results.Ok(response);

		});

		group.MapGet("/medication", async (IMediator _mediator, [FromQuery]Guid id, [FromQuery]int offset) =>
		{
			var query = new GetMedicationNotificationByMedicationQuery(id, offset);
			var response = await _mediator.Send(query);
			return Results.Ok(response);
		}).Produces(StatusCodes.Status200OK,typeof(IEnumerable<MedicationNotificationWithDateRangeDto>));

		group.MapGet("/dates/{medicationId:guid}", async (IMediator _mediator, Guid medicationId) =>
		{
			var query = new GetMedicationDateRangeQuery(medicationId);
			var response = await _mediator.Send(query);
			return response is not null ? Results.Ok(response) : Results.BadRequest();
		});

		group.MapDelete("{id::guid}", async (IMediator _mediator, Guid id) =>
		{
			var command = new DeleteMedicationNotificationCommand(id);
			await _mediator.Send(command);
			return Results.NoContent();
		});

		group.MapPatch("/", async (IMediator _mediator, EditMedicationNotificationCommand command) =>
		{
			await _mediator.Send(command);
			return Results.NoContent();
		});

		group.MapGet("/", async (IMediator _mediator, [FromQuery] int Page,
			[FromQuery] int PageSize, [FromQuery] int Offset, [FromQuery]DateTime Date) =>
		{
			var query = new GetUpcomingMedicationNotificationPageQuery(Page, PageSize,Offset,Date);
			var response = await _mediator.Send(query);
			return Results.Ok(response);
		});
	}
}
