using MediatR;
using MedicalAssistant.Application.MedicationRecommendations.Commands.AddRecommendation;
using MedicalAssistant.Application.MedicationRecommendations.Queries;
using System.Dynamic;

namespace MedicalAssistant.API.Endpoints;

public sealed class RecommendationEndpoints : IEndpoints
{
	public void MapEndpoints(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("recommendation")
			.RequireAuthorization(Permissions.Permissions.VerifiedUser)
			.WithTags("Medication Recommendations");

		group.MapPost("add", async (IMediator _mediator,
			 AddMedicationRecommendationCommand command) =>
		{
			var response = await _mediator.Send(command);
			return Results.Created($"recommendation/{response.Id}",response);
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

		//group.MapGet("/", async (
		//	IMediator _mediator,
		//	Guid visitId) 
		//		=> Results.Ok(await _mediator.Send(new GetRecommendationsForVisitQuery(visitId))));

		//group.MapDelete("{recommendationId:guid}", async (IMediator _mediator,
		//	[FromRoute]Guid visitId,
		//	[FromRoute] Guid recommendationId) =>
		//{
		//	await _mediator.Send(new DeleteRecommendationCommand(visitId, recommendationId));
		//	return Results.NoContent();
		//});

		//group.MapGet("period", async (IMediator _mediator,
		//	[FromQuery] DateTime begin, 
		//	[FromQuery] DateTime end) =>
		//{
		//	var query = new GetRecommendationsForGivenTimePeriodQuery(guid, begin, end);
		//	var response = await _mediator.Send(query);
		//	return Results.Ok(response);
		//});


	}
}
