using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MedicalAssistant.API.Tests.Abstractions;
using MedicalAssistant.Application.User.Commands.SignIn;
using MedicalAssistant.Application.User.Commands.SignUp;
using Xunit.Abstractions;

namespace MedicalAssistant.API.Tests.UserTests;

public class SignInTests(TestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    private readonly ITestOutputHelper _output;

    [Fact]
    public async Task Should_ReturnBadRequest_When_User_Is_Not_Verified()
    {
        // Arrange
        var request = new SignUpCommand("Johny Fullname", "test@testadrew.com", "password123");
        await HttpClient.PostAsJsonAsync("api/user/sign-up", request);

        var signInRequest = new SignInCommand(request.Email, request.Password);
        
        // Act
        var response = await HttpClient.PostAsJsonAsync("api/user/sign-in", signInRequest);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_ReturnOkStatus_When_User_Is_Valid_And_Verified()
    {
        await InitializeUsersAsync();
    }
    
    [Fact]
    public async Task Should_ReturnBadRequestStatus_When_User_Not_Exists()
    {
        // arrange
        string correctEmail = "test@test.com";
        string correctPassword = "password123";
        await InitializeUsersAsync(correctEmail, correctPassword);
        
        var request = new SignInCommand("test@testa.com", correctPassword);

        // act
        var response = await HttpClient.PostAsJsonAsync("api/user/sign-in",request);
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}