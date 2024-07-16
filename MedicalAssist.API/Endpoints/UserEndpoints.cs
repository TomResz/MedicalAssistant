using MediatR;
using MedicalAssist.Application.User.Commands.FacebookAuthentication;
using MedicalAssist.Application.User.Commands.GoogleAuthentication;
using MedicalAssist.Application.User.Commands.PasswordChangeByCode;
using MedicalAssist.Application.User.Commands.PasswordChangeByEmail;
using MedicalAssist.Application.User.Commands.RefreshToken;
using MedicalAssist.Application.User.Commands.RegenerateVerificationCode;
using MedicalAssist.Application.User.Commands.RevokeRefreshToken;
using MedicalAssist.Application.User.Commands.SignIn;
using MedicalAssist.Application.User.Commands.SignUp;
using MedicalAssist.Application.User.Commands.VerifyAccount;
using MedicalAssist.Application.User.Queries;
using MedicalAssist.Infrastructure.Middleware;

namespace MedicalAssist.API.Endpoints;

public sealed class UserEndpoints : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
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

		group.MapPut("revoke", async (IMediator _mediator) =>
		{
			await _mediator.Send(new RevokeRefreshTokenCommand());
			return Results.NoContent();	
		}).RequireAuthorization();

		group.MapPost("login-google", async (IMediator _mediator, GoogleAuthenticationCommand command) =>
		{
			var respone = await _mediator.Send(command);	
			return Results.Ok(respone);
		}).Produces<SignInResponse>(StatusCodes.Status200OK)
		.Produces<ErrorDetails>(StatusCodes.Status400BadRequest);

		group.MapPost("login-facebook", async (IMediator _mediator, FacebookAuthenticationCommand command) =>
		{
			var respone = await _mediator.Send(command);
			return Results.Ok(respone);
		}).Produces<SignInResponse>(StatusCodes.Status200OK)
		.Produces<ErrorDetails>(StatusCodes.Status400BadRequest);
	}
}
