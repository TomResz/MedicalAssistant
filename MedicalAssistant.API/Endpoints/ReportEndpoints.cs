using MediatR;
using MedicalAssistant.Application.Reports.MedicalHistory;
using MedicalAssistant.Application.Reports.Notes;
using MedicalAssistant.Application.Reports.Visit;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAssistant.API.Endpoints;

public class ReportEndpoints : IEndpoints
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("report")
            .RequireAuthorization(Permissions.Permissions.IsVerifiedAndActive)
            .WithTags("PDF Report");

        group.MapGet("/visit", async ([FromQuery(Name = "id")]Guid[] ids,IMediator mediator) =>
        {
            var command = new CreateVisitReportCommand(ids.ToList());
            var response = await mediator.Send(command);
            if (response is null)
            {
                return Results.NotFound();
            }
            return Results.File(response.Content, "application/pdf",response.Name);
        });
        
        group.MapGet("/medical-history", async ([FromQuery(Name = "id")]Guid[] ids,IMediator mediator) =>
        {
            var command = new CreateMedicalHistoryReportCommand(ids.ToList());
            var response = await mediator.Send(command);
            
            if (response is null)
            {
                return Results.NotFound();
            }
            return Results.File(response.Content, "application/pdf",response.Name);
        });
        
        group.MapGet("/notes", async ([FromQuery(Name = "id")]Guid[] ids,IMediator mediator) =>
        {
            var command = new CreateNoteReportCommand(ids.ToList());
            var response = await mediator.Send(command);
            
            if (response is null)
            {
                return Results.NotFound();
            }
            
            return Results.File(response.Content, "application/pdf",response.Name);
        });
    }
    
}