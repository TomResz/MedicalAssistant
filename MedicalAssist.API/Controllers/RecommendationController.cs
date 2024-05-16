using MediatR;
using MedicalAssist.API.Models;
using MedicalAssist.Application.Dto;
using MedicalAssist.Application.Recommendations.Commands.AddRecommendation;
using MedicalAssist.Application.Recommendations.Commands.DeleteRecommendation;
using MedicalAssist.Application.Recommendations.Queries;
using MedicalAssist.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAssist.API.Controllers;

[Authorize(Roles ="user",Policy =CustomClaim.IsVerified)]
[Route("api/{visitId:guid}/[controller]")]
[ApiController]
public class RecommendationController : ControllerBase
{
	private readonly IMediator _mediator;

	public RecommendationController(IMediator mediator) => _mediator = mediator;

	[HttpPost("add")]
	public async Task<IActionResult> AddRecommendation([FromRoute]Guid visitId, [FromBody]AddRecommendationModel model)
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
		return NoContent();
	}

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RecommendationDto>>> Get([FromRoute] Guid visitId) 
		=> Ok(await _mediator.Send(new GetRecommendationsForVisitQuery(visitId)));

	[HttpDelete("{recommendationId:guid}")]
	public async Task<IActionResult> Delete([FromRoute]Guid visitId, [FromRoute]Guid recommendationId)
	{
		await _mediator.Send(new DeleteRecommendationCommand(visitId, recommendationId));
		return NoContent();
	}

	[HttpGet("period")]
	public async Task<ActionResult<IEnumerable<RecommendationDto>>> GetRecommendationForTimePeriod([FromRoute]Guid visitId, [FromQuery]DateTime begin, [FromQuery]DateTime end)
		=> Ok(await _mediator.Send(new GetRecommendationsForGivenTimePeriodQuery(visitId, begin, end)));
}
