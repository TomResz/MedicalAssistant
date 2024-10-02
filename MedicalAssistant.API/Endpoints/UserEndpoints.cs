using MediatR;
using MedicalAssistant.Application.Security;
using MedicalAssistant.Application.User.Commands.FacebookAuthentication;
using MedicalAssistant.Application.User.Commands.GoogleAuthentication;
using MedicalAssistant.Application.User.Commands.PasswordChange;
using MedicalAssistant.Application.User.Commands.PasswordChangeByCode;
using MedicalAssistant.Application.User.Commands.PasswordChangeByEmail;
using MedicalAssistant.Application.User.Commands.RefreshToken;
using MedicalAssistant.Application.User.Commands.RegenerateVerificationCode;
using MedicalAssistant.Application.User.Commands.RevokeRefreshToken;
using MedicalAssistant.Application.User.Commands.SignIn;
using MedicalAssistant.Application.User.Commands.SignUp;
using MedicalAssistant.Application.User.Commands.VerifyAccount;
using MedicalAssistant.Application.User.Queries;
using MedicalAssistant.Infrastructure.Middleware;

namespace MedicalAssistant.API.Endpoints;

public sealed class UserEndpoints : IEndpoints
{
	public void MapEndpoints(IEndpointRouteBuilder app)
	{

		var group = app.MapGroup("user").WithTags("Users");

		group.MapGet("get", async (IMediator _mediator) =>
		{
			return Results.Ok(await _mediator.Send(new GetUserCredentialsQuery()));
		}).RequireAuthorization();

		group.MapPost("sign-up", async (IMediator _mediator, SignUpCommand command) =>
		{
			await _mediator.Send(command);
			return Results.NoContent();	
		});

		group.MapPost("sign-in", async (IMediator _mediator, SignInCommand command) =>
		{
			var response = await _mediator.Send(command);
			return Results.Ok(response);
		});

		group.MapPut("verify", async (IMediator _mediator, VerifyAccountCommand command) =>
		{
			await _mediator.Send(command);
			return Results.NoContent();
		});

		group.MapPut("regenerate-code", async (IMediator _mediator, RegenerateVerificationCodeCommand command) =>
		{
			await _mediator.Send(command);
			return Results.NoContent();
		});

		group.MapPost("password-change", async (IMediator _mediator, PasswordChangeByEmailCommand command) =>
		{
			await _mediator.Send(command);
			return Results.NoContent();
		});

		group.MapPut("password-change-auth", async (IMediator _mediator, ChangePasswordCommand command) =>
		{
			await _mediator.Send(command);
			return Results.NoContent();
		});

		group.MapPost("check-password-code/{code}", (IEmailCodeManager manager,string code) => 
			manager.Decode(code,out var email) ? Results.Ok() : Results.BadRequest());

		group.MapPut("password-change-by-code", async (IMediator _mediator, PasswordChangeByCodeCommand command) =>
		{
			await _mediator.Send(command);
			return Results.NoContent();
		});

		group.MapPost("refresh-token", async (IMediator _mediator, RefreshTokenCommand command) =>
		{
			var response = await _mediator.Send(command);
			return Results.Ok(response);
		});

		group.MapPut("revoke", async (IMediator _mediator, RevokeRefreshTokenCommand command) =>
		{
			await _mediator.Send(command);
			return Results.NoContent();	
		}).RequireAuthorization(Permissions.Permissions.VerifiedUser);

		group.MapPost("login-google", async (IMediator _mediator, GoogleAuthenticationCommand command) =>
		{
			var respone = await _mediator.Send(command);	
			return Results.Ok(respone);
		}).Produces<SignInResponse>(StatusCodes.Status200OK)
		.Produces<ErrorDetails>(StatusCodes.Status409Conflict);

		group.MapPost("login-facebook", async (IMediator _mediator, FacebookAuthenticationCommand command) =>
		{
			var respone = await _mediator.Send(command);
			return Results.Ok(respone);
		}).Produces<SignInResponse>(StatusCodes.Status200OK)
		.Produces<ErrorDetails>(StatusCodes.Status409Conflict);
	}
}
