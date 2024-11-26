using MediatR;
using MedicalAssistant.Application.Reports.MedicalHistory;
using MedicalAssistant.Application.Reports.Visit;

namespace MedicalAssistant.API.Endpoints;

public class ReportEndpoints : IEndpoints
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("report")
            .RequireAuthorization(Permissions.Permissions.IsVerifiedAndActive)
            .WithTags("PDF Report");

        group.MapGet("/visit", async (IMediator mediator) =>
        {
            var command = new CreateVisitReportCommand();
            var response = await mediator.Send(command);
            if (response is null)
            {
                return Results.NotFound();
            }
            
            return Results.File(response.Content, "application/pdf",response.Name);
        });
        
        group.MapGet("/history", async (IMediator mediator) =>
        {
            var command = new CreateMedicalHistoryReportCommand();
            var response = await mediator.Send(command);
            
            if (response is null)
            {
                return Results.NotFound();
            }
            
            return Results.File(response.Content, "application/pdf",response.Name);
        });
    }
    
}