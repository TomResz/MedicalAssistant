using MediatR;
using MedicalAssistant.Application.Security;
using MedicalAssistant.Application.User.Commands.DeactivateAccount;
using MedicalAssistant.Application.User.Commands.DeleteAccount;
using MedicalAssistant.Application.User.Commands.FacebookAuthentication;
using MedicalAssistant.Application.User.Commands.GoogleAuthentication;
using MedicalAssistant.Application.User.Commands.PasswordChange;
using MedicalAssistant.Application.User.Commands.PasswordChangeByCode;
using MedicalAssistant.Application.User.Commands.PasswordChangeByEmail;
using MedicalAssistant.Application.User.Commands.ReactivateAccount;
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

		group.MapGet("get", async (IMediator mediator) => Results.Ok(await mediator.Send(new GetUserCredentialsQuery()))).RequireAuthorization();

		group.MapPost("sign-up", async (IMediator mediator, SignUpCommand command) =>
		{
			await mediator.Send(command);
			return Results.NoContent();	
		});

		group.MapPost("sign-in", async (IMediator mediator, SignInCommand command) =>
		{
			var response = await mediator.Send(command);
			return Results.Ok(response);
		});

		group.MapPut("verify", async (IMediator mediator, VerifyAccountCommand command) =>
		{
			await mediator.Send(command);
			return Results.NoContent();
		});

		group.MapPut("regenerate-code", async (IMediator mediator, RegenerateVerificationCodeCommand command) =>
		{
			await mediator.Send(command);
			return Results.NoContent();
		});

		group.MapPost("password-change", async (IMediator mediator, PasswordChangeByEmailCommand command) =>
		{
			await mediator.Send(command);
			return Results.NoContent();
		});

		group.MapPut("password-change-auth", async (IMediator mediator, ChangePasswordCommand command) =>
		{
			await mediator.Send(command);
			return Results.NoContent();
		});

		group.MapPost("check-password-code/{code}", (IEmailCodeManager manager,string code) => 
			manager.Decode(code,out var email) ? Results.Ok() : Results.BadRequest());

		group.MapPut("password-change-by-code", async (IMediator mediator, PasswordChangeByCodeCommand command) =>
		{
			await mediator.Send(command);
			return Results.NoContent();
		});

		group.MapPost("refresh-token", async (IMediator mediator, RefreshTokenCommand command) =>
		{
			var response = await mediator.Send(command);
			return Results.Ok(response);
		});

		group.MapPut("revoke", async (IMediator mediator, RevokeRefreshTokenCommand command) =>
		{
			await mediator.Send(command);
			return Results.NoContent();	
		}).RequireAuthorization(Permissions.Permissions.IsVerifiedAndActive);

		group.MapPost("login-google", async (IMediator mediator, GoogleAuthenticationCommand command) =>
		{
			var response = await mediator.Send(command);	
			return Results.Ok(response);
		}).Produces<SignInResponse>(StatusCodes.Status200OK)
		.Produces<ErrorDetails>(StatusCodes.Status409Conflict);

		group.MapPost("login-facebook", async (IMediator mediator, FacebookAuthenticationCommand command) =>
		{
			var response = await mediator.Send(command);
			return Results.Ok(response);
		}).Produces<SignInResponse>(StatusCodes.Status200OK)
		.Produces<ErrorDetails>(StatusCodes.Status409Conflict);

		group.MapDelete("/", async (IMediator mediator) =>
		{
			var command = new DeleteAccountCommand();
			await mediator.Send(command);
			return Results.NoContent();
		}).RequireAuthorization(Permissions.Permissions.IsVerifiedAndActive);

		group.MapPost("/reactivate", async (IMediator mediator) =>
		{
			var command = new ReactivateAccountCommand();
			await mediator.Send(command);
			return Results.NoContent();
		}).RequireAuthorization(Permissions.Permissions.NotActiveUser);
		
		group.MapPost("/deactivate", async (IMediator mediator) =>
		{
			var command = new DeactivateAccountCommand();
			await mediator.Send(command);
			return Results.NoContent();
		}).RequireAuthorization(Permissions.Permissions.IsVerifiedAndActive);
	}
}
