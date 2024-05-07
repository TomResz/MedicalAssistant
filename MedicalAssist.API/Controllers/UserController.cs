using MediatR;
using MedicalAssist.Application.Dto;
using MedicalAssist.Application.User.Commands.PasswordChangeByCode;
using MedicalAssist.Application.User.Commands.PasswordChangeByEmail;
using MedicalAssist.Application.User.Commands.RegenerateVerificationCode;
using MedicalAssist.Application.User.Commands.SignIn;
using MedicalAssist.Application.User.Commands.SignUp;
using MedicalAssist.Application.User.Commands.VerifyAccount;
using MedicalAssist.Application.User.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAssist.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator) => _mediator = mediator;

    [HttpGet("get")]
    public async Task<ActionResult<UserCredentialsDto>> GetCredentials()
        => Ok(await _mediator.Send(new GetUserCredentialsQuery()));


    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp(SignUpCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult<SignInResponse>> SignIn(SignInCommand command)
        => Ok(await _mediator.Send(command));

    [HttpPut("verify")]
    public async Task<IActionResult> VerifyAccount(VerifyAccountCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

	[HttpPut("regenerate-code")]
	public async Task<IActionResult> RegenerateCode(RegenerateVerificationCodeCommand command)
	{
		await _mediator.Send(command);
		return NoContent();
	}

    [HttpPost("password-change")]
    public async Task<IActionResult> PasswordChange(PasswordChangeByEmailCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("password-change-by-code")]
    public async Task<IActionResult> PasswordChangeByCode(PasswordChangeByCodeCommand command )
    {
        await _mediator.Send(command);
        return NoContent();
    }
}
