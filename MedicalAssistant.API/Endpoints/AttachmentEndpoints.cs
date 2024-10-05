using MediatR;
using MedicalAssistant.Application.Attachment.Commands.Add;
using MedicalAssistant.Application.Attachment.Commands.Delete;
using MedicalAssistant.Application.Attachment.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAssistant.API.Endpoints;

public class AttachmentEndpoints : IEndpoints
{
	public void MapEndpoints(IEndpointRouteBuilder app)
	{
		var groupWithoutVisit = app.MapGroup("attachment")
			.WithTags("Attachments")
			.RequireAuthorization(Permissions.Permissions.VerifiedUser);

		groupWithoutVisit.MapGet("{id:guid}", async (IMediator _mediator, Guid id,HttpResponse httpResponse) =>
		{
			var query = new DownloadAttachmentByIdQuery(id);
			var response = await _mediator.Send(query);

			if (response is null)
			{
				return Results.NotFound();
			}

			var mimeType = response.FileExtension.ToLower() switch
			{
				".jpg" or ".jpeg" => "image/jpeg",
				".png" => "image/png",
				_ => "application/pdf",
			};

			httpResponse.Headers.Append("X-FileName", response.Name);
			return Results.File(response.Content, mimeType, response.Name);
		});

		groupWithoutVisit.MapDelete("{id:guid}", async (IMediator _mediator, Guid id) =>
		{
			var command = new DeleteAttachmentCommand(id);
			await _mediator.Send(command);
			return Results.NoContent();
		});


		var group = app.MapGroup("{visitId:guid}/attachment")
			.WithTags("Attachments")
			.RequireAuthorization(Permissions.Permissions.VerifiedUser);

		group.MapPost("/", async (
			[FromRoute] Guid visitId,
			[FromForm] IFormFile file,
			IMediator _mediator) =>
		{
			var command = new AddAttachmentCommand(visitId, file);
			var response = await _mediator.Send(command);
			return Results.Created($"/api/attachment/{response.Id}", response);
		}).DisableAntiforgery();

		group.MapGet("/", async (IMediator _mediator, [FromRoute] Guid visitId) =>
		{
			var query = new GetAttachmentViewListQuery(visitId);
			var response = await _mediator.Send(query);
			return Results.Ok(response);
		});
	}
}
