using MediatR;
using MedicalAssistant.API.Models;
using MedicalAssistant.API.Models.DiseaseStages;
using MedicalAssistant.Application.MedicalHistory.Commands.Add;
using MedicalAssistant.Application.MedicalHistory.Commands.AddStage;
using MedicalAssistant.Application.MedicalHistory.Commands.Delete;
using MedicalAssistant.Application.MedicalHistory.Commands.DeleteStage;
using MedicalAssistant.Application.MedicalHistory.Commands.Edit;
using MedicalAssistant.Application.MedicalHistory.Commands.EditStage;
using MedicalAssistant.Application.MedicalHistory.Query;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAssistant.API.Endpoints;

public class MedicalHistoryEndpoints : IEndpoints
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("medicalhistory")
            .WithTags("Medical History")
            .RequireAuthorization(Permissions.Permissions.IsVerifiedAndActive);

        group.MapPost("/", async (IMediator mediator, AddMedicalHistoryCommand command) =>
        {
            var response = await mediator.Send(command);
            return Results.Created($"api/medicalhistory/{response}", response);
        }).Produces(StatusCodes.Status201Created, typeof(Guid));

        group.MapPost("/{id:guid}/stage", async (IMediator mediator, Guid id, AddDiseaseStageModel model) =>
        {
            var command = new AddDiseaseStageCommand(
                id,
                model.VisitId,
                model.Note,
                model.Name,
                model.Date);

            var response = await mediator.Send(command);

            return Results.Created($"api/medicalhistory/stage/{response}", response);
        });

        group.MapGet("/", async (IMediator mediator) =>
        {
            var query = new GetMedicalHistoriesQuery();
            var response = await mediator.Send(query);
            return Results.Ok(response);
        });

        group.MapGet("/{searchTerm}", async (IMediator mediator, string searchTerm) =>
        {
            var query = new SearchMedicalHistoriesBySearchTermQuery(searchTerm);
            var response = await mediator.Send(query);
            return Results.Ok(response);
        });

        group.MapGet("{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var query = new GetMedicalHistoryByIdQuery(id);
            var response = await mediator.Send(query);
            return Results.Ok(response);
        });

        group.MapGet("/stage/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var query = new GetDiseaseStageQuery(id);
            var response = await mediator.Send(query);
            return Results.Ok(response);
        });

        group.MapDelete("/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var command = new DeleteMedicalHistoryCommand(id);
            await mediator.Send(command);
            return Results.NoContent();
        });

        group.MapPatch("/", async (IMediator mediator, EditMedicalHistoryCommand command) =>
        {
            await mediator.Send(command);
            return Results.NoContent();
        });

        group.MapPatch("/{medicalHistoryId:guid}/stage", async (
            IMediator mediator, [FromRoute] Guid medicalHistoryId,
            [FromBody] EditDiseaseStageModel model) =>
        {
            var command = new EditDiseaseStageCommand(
                model.Id,
                medicalHistoryId,
                model.VisitId,
                model.Name,
                model.Note,
                model.Date);

            await mediator.Send(command);
            return Results.NoContent();
        });

        group.MapDelete("/{medicalHistoryId:guid}/stage/{id:guid}",async (
            IMediator mediator, Guid id, Guid medicalHistoryId) =>
        {
            var command = new DeleteDiseaseStageCommand(id, medicalHistoryId);
            await mediator.Send(command);
            return Results.NoContent();
        });
    }
}