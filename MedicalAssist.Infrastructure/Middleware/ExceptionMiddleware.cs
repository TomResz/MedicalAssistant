using MedicalAssist.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Authentication;
using System.Text.Json;

namespace MedicalAssist.Infrastructure.Middleware;
internal sealed class ExceptionMiddleware : IMiddleware
{
	private readonly ILogger<ExceptionMiddleware> _logger;

	public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger) 
		=> _logger = logger;

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next.Invoke(context);
		}
		catch (Exception ex)
		{
			var statusCode = ex switch
			{
				BadRequestException => StatusCodes.Status400BadRequest,
				ConflictException => StatusCodes.Status409Conflict,
				AuthenticationException => StatusCodes.Status401Unauthorized,
				_ => StatusCodes.Status500InternalServerError
			};
			await HandleError(statusCode, ex, context);
		}
	}

	private async Task HandleError(int statusCode, Exception ex, HttpContext context)
	{
		var type = ex
			.GetType()
			.ToString()
			.Split('.')
			.Last()
			.Replace(nameof(Exception), "");

		var details = new ErrorDetails(statusCode, type, ex.Message);

		string content = JsonSerializer.Serialize(details);

		_logger.LogError($"Exception caught: {content}");

		if (statusCode == StatusCodes.Status500InternalServerError)
		{
			var criticalErrorDetails = new CriticalErrorDetails(details, ex.StackTrace ?? "");
			_logger.LogError($"Critical error stack trace: {criticalErrorDetails.StackTrace}.");
		}

		context.Response.ContentType = "application/json";
		context.Response.StatusCode = statusCode;
		await context.Response.WriteAsync(content);
	}
}
