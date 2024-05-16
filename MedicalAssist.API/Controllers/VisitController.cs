using MediatR;
using MedicalAssist.API.QueryPolicy;
using MedicalAssist.Application.Dto;
using MedicalAssist.Application.Pagination;
using MedicalAssist.Application.Visits.Commands.AddVisit;
using MedicalAssist.Application.Visits.Commands.ConfirmVisit;
using MedicalAssist.Application.Visits.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MedicalAssist.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VisitController : ControllerBase
{
	private readonly IMediator _mediator;

	public VisitController(IMediator mediator) => _mediator = mediator;

	[HttpPost("add")]
	public async Task<IActionResult> AddVisit(AddVisitCommand command)
	{
		await _mediator.Send(command);
		return Created("api/Visit/add", null);
	}

	[HttpGet("current")]
	public async Task<ActionResult<PagedList<VisitDto>>> GetCurrentVisits([FromQuery]PageParameters pageParameters, [FromQuery]string direction = "asc")
	{
		var order = SortingParameters.FromString(direction);
		var query = new GetPageOfCurrentVisitsQuery(pageParameters.Page, pageParameters.PageSize,order);
		return await _mediator.Send(query);
	}

	[HttpPut("confirm")]
	public async Task<IActionResult> ConfirmVisit([FromBody]ConfirmVisitCommand command)
	{
		await _mediator.Send(command);
		return NoContent();
	}

	[HttpGet("{id:guid}")]
	public async Task<ActionResult<VisitDto>> Get(Guid id)
		=> Ok(await _mediator.Send(new GetDetailsOfVisitQuery(id)));

    [HttpGet("calendar")]
    public async Task<ActionResult<IEnumerable<VisitDateAvailableCountDto>>> GetCount()
		=> Ok(await _mediator.Send(new GetCountOfVisitsQuery()));

    [HttpGet("month={month:int}")]
    public async Task<ActionResult<IEnumerable<VisitDateAvailableCountDto>>> GetCountByMount([FromRoute][Range(1,12)]int month)
		=> Ok(await _mediator.Send(new GetCountOfVisitsByMonthQuery(month)));

	[HttpGet("day={day}")]
	public async Task<ActionResult<IEnumerable<VisitDto>>> GetVisitsByDay(DateTime day)
		=> Ok(await _mediator.Send(new GetVisitsByDayQuery(day)));
}
