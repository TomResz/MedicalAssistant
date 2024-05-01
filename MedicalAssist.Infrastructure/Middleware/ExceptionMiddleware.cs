using MedicalAssist.Domain.Exceptions.Shared;
using Microsoft.AspNetCore.Http;
using System.Security.Authentication;
using System.Text.Json;

namespace MedicalAssist.Infrastructure.Middleware;
internal sealed class ExceptionMiddleware : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next.Invoke(context);
		}
		catch(BadRequestException ex)
		{
			await HandleError(StatusCodes.Status400BadRequest,ex,context);
		}
		catch(AuthenticationException ex)
		{
			await HandleError(StatusCodes.Status401Unauthorized,ex,context);
		}
		catch (Exception ex) 
		{
			await HandleError(StatusCodes.Status500InternalServerError, ex, context);
		}
	}

	private async Task HandleError(int statusCode,Exception ex, HttpContext context)
	{
		var type = ex
			.GetType()
			.ToString()
			.Split('.')
			.Last();

		string content = "";
		if(statusCode == 500)
		{
			content = new CriticalErrorDetails(statusCode, type,ex.Message,ex.StackTrace ?? "").ToString();
		}
		else
		{
			content = new ErrorDetails(statusCode, type, ex.Message).ToString();
		}

		context.Response.ContentType = "application/json";
		context.Response.StatusCode = statusCode;
		await context.Response.WriteAsync(content);
	}
}
