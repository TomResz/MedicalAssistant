using MediatR;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.UserSettings.Commands.Update;
using MedicalAssistant.Application.UserSettings.Queries;
using MedicalAssistant.Infrastructure.Middleware;
using MedicalAssistant.UI.Shared.Response.Base;

namespace MedicalAssistant.API.Endpoints;

public class UserSettingsEndpoints : IEndpoints
{
	public void MapEndpoints(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("settings")
			.WithTags("User Settings")
			.RequireAuthorization(Permissions.Permissions.VerifiedUser);

		group.MapGet("/", async (IMediator _mediator) =>
		{
			var query = new GetUserSettingsQuery();
			var response = await _mediator.Send(query);
			return Results.Ok(response);
		}).Produces(StatusCodes.Status200OK,typeof(SettingsDto))
		.Produces(StatusCodes.Status400BadRequest,typeof(ErrorDetails));
		
		group.MapPatch("/update", async (IMediator _mediator, UpdateUserSettingsCommand command) =>
		{
			await _mediator.Send(command);
			return Results.NoContent();	
		}).Produces(StatusCodes.Status204NoContent)
		.Produces(StatusCodes.Status400BadRequest, typeof(BaseErrorDetails));
	}
}
