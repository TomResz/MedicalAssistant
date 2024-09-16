﻿using MediatR;
using MedicalAssistant.API.Models;
using MedicalAssistant.Application.Recommendations.Commands.AddRecommendation;
using MedicalAssistant.Application.Recommendations.Commands.DeleteRecommendation;
using MedicalAssistant.Application.Recommendations.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAssistant.API.Endpoints;

public sealed class RecommendationEndpoints : IEndpoints
{
	public void MapEndpoints(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("{visitId:guid}/recommendation")
			.RequireAuthorization(Permissions.Permissions.VerifiedUser)
			.WithTags("Recommendations");

		group.MapPost("add", async (IMediator _mediator,
			[FromRoute]Guid visitId,
			[FromBody] AddRecommendationModel model) =>
		{
			var command = new AddRecommendationCommand(
		visitId,
	 model.ExtraNote,
	   model.MedicineName,
		  model.Quantity,
			model.TimeOfDay,
			model.StartDate,
			 model.EndDate);
			await _mediator.Send(command);
			return Results.NoContent();
		});

		group.MapGet("/", async (
			IMediator _mediator,
			Guid visitId) 
				=> Results.Ok(await _mediator.Send(new GetRecommendationsForVisitQuery(visitId))));

		group.MapDelete("{recommendationId:guid}", async (IMediator _mediator,
			[FromRoute]Guid visitId,
			[FromRoute] Guid recommendationId) =>
		{
			await _mediator.Send(new DeleteRecommendationCommand(visitId, recommendationId));
			return Results.NoContent();
		});

		group.MapGet("period", async (IMediator _mediator,
			[FromRoute]Guid visitId, 
			[FromQuery] DateTime begin, 
			[FromQuery] DateTime end) =>
		{
			var query = new GetRecommendationsForGivenTimePeriodQuery(visitId, begin, end);
			var response = await _mediator.Send(query);
			return Results.Ok(response);
		});


	}
}