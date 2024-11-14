using MediatR;
using MedicalAssistant.API.Models;
using MedicalAssistant.Application.MedicalHistory.Commands.Add;
using MedicalAssistant.Application.MedicalHistory.Commands.AddStage;
using MedicalAssistant.Application.MedicalHistory.Query;

namespace MedicalAssistant.API.Endpoints;

public class MedicalHistoryEndpoints : IEndpoints
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("medicalhistory")
            .WithTags("Medical History")
            .RequireAuthorization(Permissions.Permissions.VerifiedUser);

        group.MapPost("/", async (IMediator mediator,AddMedicalHistoryCommand command) =>
        {
            var response = await mediator.Send(command);
            return Results.Created($"api/medicalhistory/{response}", response);
        }).Produces(StatusCodes.Status201Created,typeof(Guid));

        group.MapPost("/{id:guid}/stage", async (IMediator mediator, Guid id,AddDiseaseStageModel model) =>
        {
            var command = new AddDiseaseStageCommand(id,
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
    }
}