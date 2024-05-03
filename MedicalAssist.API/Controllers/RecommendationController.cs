using MediatR;
using MedicalAssist.API.Models;
using MedicalAssist.Application.Recommendations.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAssist.API.Controllers;
[Authorize]
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
					model.TimeOfDay);
		await _mediator.Send(command);
		return NoContent();
	}
}
