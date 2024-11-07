using MediatR;
using MedicalAssistant.Application.MedicalHistory.Commands.Add;

namespace MedicalAssistant.API.Endpoints;

public class MedicalHistoryEndpoints : IEndpoints
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/medicalhistory")
            .WithTags("Medical History")
            .RequireAuthorization(Permissions.Permissions.VerifiedUser);

        group.MapPost("/", async (IMediator mediator,AddMedicalHistoryCommand command) =>
        {
            var response = await mediator.Send(command);
            return Results.Created($"api/medicalhistory/{response}", response);
        }).Produces(StatusCodes.Status201Created,typeof(Guid));
    }
}