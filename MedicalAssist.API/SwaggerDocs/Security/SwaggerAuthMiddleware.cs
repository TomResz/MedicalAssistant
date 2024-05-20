using System.Net;
using System.Text;

namespace MedicalAssist.API.SwaggerDocs.Security;

public class SwaggerAuthMiddleware(IConfiguration _configuration) : IMiddleware
{
    private readonly string _userName = _configuration["Swagger:Username"] ?? throw new ArgumentNullException(nameof(_userName));
    private readonly string _password = _configuration["Swagger:Password"] ?? throw new ArgumentNullException(nameof(_password));

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            string? authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();

                var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

                var username = decodedUsernamePassword.Split(':', 2)[0];
                var password = decodedUsernamePassword.Split(':', 2)[1];

                if (IsAuthorized(username, password))
                {
                    await next.Invoke(context);
                    return;
                }
            }
            context.Response.Headers["WWW-Authenticate"] = "Basic";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
        else
        {
            await next.Invoke(context);
        }
    }

    private bool IsAuthorized(string username, string password) 
        => username.Equals(_userName) && password.Equals(_password);
}
