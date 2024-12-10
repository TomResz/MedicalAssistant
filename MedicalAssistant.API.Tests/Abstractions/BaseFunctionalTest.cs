using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.User.Commands.SignIn;
using MedicalAssistant.Application.User.Commands.SignUp;
using MedicalAssistant.Application.User.Commands.VerifyAccount;
using MedicalAssistant.Infrastructure.DAL;
using MedicalAssistant.Infrastructure.Middleware;
using MedicalAssistant.UI.Shared.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;
using SignInResponse = MedicalAssistant.Application.User.Commands.SignIn.SignInResponse;

namespace MedicalAssistant.API.Tests.Abstractions;

public class BaseFunctionalTest : IClassFixture<TestWebAppFactory>
{
    public BaseFunctionalTest(TestWebAppFactory applicationFactory)
    {
        ApplicationFactory = applicationFactory;
        HttpClient = applicationFactory.CreateClient();
        HttpClient.DefaultRequestHeaders.Add("X-Current-Language", "pl-PL");
    }
    protected TestWebAppFactory ApplicationFactory { get; init; }
    protected HttpClient HttpClient { get; init; }
    protected SignInResponse TokenHolder { get; private set; }

    protected async Task InitializeUsersAsync(string email = "johny@gmail.com", string password = "password123")
    {
        // Create User
        var request = new SignUpCommand("Johny Fullname", email, password);
        await HttpClient.PostAsJsonAsync("api/user/sign-up", request);

        // Verify User
        using (var scope = ApplicationFactory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<MedicalAssistantDbContext>();

            var code = await context
                .Users
                .Include(x => x.UserVerification)
                .Where(x => x.Email == request.Email)
                .Select(x => x.UserVerification!.CodeHash)
                .FirstOrDefaultAsync();

            Assert.NotNull(code);
            var verifyCommand = new VerifyAccountCommand(code);
            var verifyResponse = await HttpClient.PutAsJsonAsync("api/user/verify", verifyCommand);
            verifyResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        // Sign In
        var signInRequest = new SignInCommand(request.Email, request.Password);
        var response = await HttpClient.PostAsJsonAsync("api/user/sign-in", signInRequest);
        var tokens = await response.Content.ReadFromJsonAsync<SignInResponse>();
        TokenHolder = tokens!;

        HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokens!.AccessToken);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
    }
}