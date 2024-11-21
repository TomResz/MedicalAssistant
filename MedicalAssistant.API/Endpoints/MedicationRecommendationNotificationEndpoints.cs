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
			.RequireAuthorization(Permissions.Permissions.IsVerifiedAndActive);

		group.MapPost("/", async (IMediator mediator, AddMedicationNotificationCommand command) =>
		{
			var id = await mediator.Send(command);
			return Results.Ok(id);
		});


		group.MapGet("week", async (IMediator mediator,
			[FromQuery]int offset, [FromQuery] DateTime date) =>
		{
			var query = new GetMedicationNotificationByWeekQuery(offset,date);
			var response = await mediator.Send(query);
			return Results.Ok(response);

		});

		group.MapGet("/medication", async (IMediator mediator, [FromQuery]Guid id, [FromQuery]int offset) =>
		{
			var query = new GetMedicationNotificationByMedicationQuery(id, offset);
			var response = await mediator.Send(query);
			return Results.Ok(response);
		}).Produces(StatusCodes.Status200OK,typeof(IEnumerable<MedicationNotificationWithDateRangeDto>));

		group.MapGet("/dates/{medicationId:guid}", async (IMediator mediator, Guid medicationId) =>
		{
			var query = new GetMedicationDateRangeQuery(medicationId);
			var response = await mediator.Send(query);
			return response is not null ? Results.Ok(response) : Results.BadRequest();
		});

		group.MapDelete("{id::guid}", async (IMediator mediator, Guid id) =>
		{
			var command = new DeleteMedicationNotificationCommand(id);
			await mediator.Send(command);
			return Results.NoContent();
		});

		group.MapPatch("/", async (IMediator mediator, EditMedicationNotificationCommand command) =>
		{
			await mediator.Send(command);
			return Results.NoContent();
		});

		group.MapGet("/", async (IMediator mediator, [FromQuery] int Page,
			[FromQuery] int PageSize, [FromQuery] int Offset, [FromQuery]DateTime Date) =>
		{
			var query = new GetUpcomingMedicationNotificationPageQuery(Page, PageSize,Offset,Date);
			var response = await mediator.Send(query);
			return Results.Ok(response);
		});

		group.MapGet("/{date:datetime}", async (IMediator mediator, DateTime date) =>
		{
			var query = new GetMedicationRecommendationByDateQuery(date);
			var response = await mediator.Send(query);
			return Results.Ok(response);	
		});
	}
}
