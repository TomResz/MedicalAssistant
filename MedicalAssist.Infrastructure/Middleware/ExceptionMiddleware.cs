using MedicalAssist.Domain.Exceptions.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Authentication;

namespace MedicalAssist.Infrastructure.Middleware;
internal sealed class ExceptionMiddleware : IMiddleware
{
	private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
    }

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
		var details = new ErrorDetails(statusCode, type, ex.Message);

        string content = details.ToString();
        
		if (statusCode == 500)
		{
			var criticalErrorDetails = new CriticalErrorDetails(details, ex.StackTrace ?? "");
			_logger.LogError(criticalErrorDetails.ToString());
		}

		context.Response.ContentType = "application/json";
		context.Response.StatusCode = statusCode;
		await context.Response.WriteAsync(content);
	}
}
