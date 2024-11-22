using MediatR;
using MedicalAssistant.Application.MedicalNotes.Commands.Add;
using MedicalAssistant.Application.MedicalNotes.Commands.Delete;
using MedicalAssistant.Application.MedicalNotes.Commands.Edit;
using MedicalAssistant.Application.MedicalNotes.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAssistant.API.Endpoints;

public class MedicalNoteEndpoints : IEndpoints
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("medicalnote")
            .WithTags("Medical Notes")
            .RequireAuthorization(Permissions.Permissions.IsVerifiedAndActive);

        group.MapPost("/", async (IMediator mediator, AddMedicalNoteCommand command) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/medicalnote/{id}", id);
        });

        group.MapGet("/", async (IMediator mediator) =>
        {
            var query = new GetMedicalNotesQuery();
            var result = await mediator.Send(query);
            return Results.Ok(result);
        });

        group.MapGet("/tags", async (IMediator mediator) =>
        {
            var query = new GetTagsOfNotesQuery();
            var result = await mediator.Send(query);
            return Results.Ok(result);
        });

        group.MapPatch("/", async (IMediator mediator, EditMedicalNoteCommand command) =>
        {
            await mediator.Send(command);
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var command = new DeleteMedicalNoteCommand(id);
            await mediator.Send(command);
            return Results.NoContent();
        });

        group.MapGet("/search", async (
            IMediator mediator, [FromQuery(Name = "tag")] string[]? tags,
            [FromQuery(Name = "term")] string? searchTerm) =>
        {
            var query = new GetMedicalNotesByTermAndTagsQuery(tags, searchTerm);
            var response = await mediator.Send(query);
            return Results.Ok(response);
        });
    }
}