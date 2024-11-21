using MediatR;
using MedicalAssistant.Application.MedicationRecommendations.Commands.AddRecommendation;
using MedicalAssistant.Application.MedicationRecommendations.Commands.DeleteRecommendation;
using MedicalAssistant.Application.MedicationRecommendations.Commands.UpdateRecommendation;
using MedicalAssistant.Application.MedicationRecommendations.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAssistant.API.Endpoints;

public sealed class RecommendationEndpoints : IEndpoints
{
	public void MapEndpoints(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("recommendation")
			.RequireAuthorization(Permissions.Permissions.IsVerifiedAndActive)
			.WithTags("Medication Recommendations");

		group.MapPost("add", async (IMediator _mediator,
			 AddMedicationRecommendationCommand command) =>
		{
			var response = await _mediator.Send(command);
			return Results.Created($"recommendation/{response.Id}", response);
		});

		group.MapGet("/{id:guid}", async (IMediator _mediator, Guid id) =>
		{
			var query = new GetMedicationRecommendationQuery(id);
			var response = await _mediator.Send(query);
			return Results.Ok(response);
		});

		group.MapGet("/", async (IMediator _mediator) =>
		{
			var query = new GetAllMedicationRecommendationsQuery();
			var response = await _mediator.Send(query);
			return Results.Ok(response);
		});

		group.MapPatch("/", async (IMediator _mediator, UpdateMedicationRecommendationCommand command) =>
		{
			var response = await _mediator.Send(command);
			return Results.Ok(response);
		});

		group.MapDelete("{id:guid}", async (IMediator _mediator, Guid id) =>
		{
			var command = new DeleteRecommendationCommand(id);
			await _mediator.Send(command);
			return Results.NoContent();
		});

		group.MapGet("{date:datetime}", async (IMediator _mediator, DateTime date) =>
		{
			var query = new GetMedicationRecommendationByDateQuery(date.Date);
			var response = await _mediator.Send(query);
			return Results.Ok(response);
		});

		group.MapGet("week/{date:datetime}", async (IMediator _mediator, [FromRoute] DateTime date) =>
		{
			var query = new GetMedicationRecommendationByWeekQuery(date.Date);
			var response = await _mediator.Send(query);	
			return Results.Ok(response);	
		});
	}
}
