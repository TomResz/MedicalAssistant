using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MedicalAssistant.API.Tests.Abstractions;
using MedicalAssistant.Application.User.Commands.PasswordChange;

namespace MedicalAssistant.API.Tests.UserTests;

public class ChangePasswordTests(TestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task Should_ReturnNoContentRequest_When_ChangePasswordSucceed()
    {
        string email = "test@test.com";
        string oldPassword = "oldPassword";
        
        await InitializeUsersAsync(email, oldPassword);

        var request = new ChangePasswordCommand("newStrongPassword", "newStrongPassword");
        
        var response = await HttpClient.PutAsJsonAsync("api/user/password-change-auth",request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task Should_ReturnBadRequestStatus_When_NewPasswordIsSameAsOldPassword()
    {
        string email = $"john{Guid.NewGuid()}@test.com";
        string oldPassword = "oldPassword";
        
        await InitializeUsersAsync(email, oldPassword);

        var request = new ChangePasswordCommand(oldPassword, oldPassword);
        
        var response = await HttpClient.PutAsJsonAsync("api/user/password-change-auth",request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
        
    [Fact]
    public async Task Should_ReturnBadRequestStatus_When_ConfirmPasswordIsNotSameAsPassword()
    {
        string email = $"john{Guid.NewGuid()}@test.com";
        string oldPassword = "oldPassword";
        
        await InitializeUsersAsync(email, oldPassword);

        var request = new ChangePasswordCommand("password123", "123password123");
        
        var response = await HttpClient.PutAsJsonAsync("api/user/password-change-auth",request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    } 
    
}