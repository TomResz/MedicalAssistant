using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MedicalAssistant.API.Tests.Abstractions;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.User.Commands.SignUp;
using Xunit.Abstractions;

namespace MedicalAssistant.API.Tests.UserTests;

public class RegisterUserTests : BaseFunctionalTest
{
    private readonly ITestOutputHelper _output;
    
    public RegisterUserTests(ITestOutputHelper output, TestWebAppFactory factory) : base(factory)
    {
        _output = output;
    }

    [Fact]
    public async Task Should_ReturnNoContentResponse_When_RegisterUser_Is_Valid()
    {
        var request = new SignUpCommand("Johny Fullname", "test@test.com", "password12345678");
        var response = await HttpClient.PostAsJsonAsync("api/user/sign-up", request);

        var message = await response.Content.ReadAsStringAsync();
        _output.WriteLine($"Response message: {message}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [InlineData("", "test@test.com", "password12345678", HttpStatusCode.BadRequest)] // empty fullname
    [InlineData("Johny Fullname", "", "password12345678", HttpStatusCode.BadRequest)] // empty email
    [InlineData("Johny Fullname", "test@test.com", "", HttpStatusCode.BadRequest)] // empty password
    [InlineData("Johny Fullname", "not-an-email", "password12345678", HttpStatusCode.BadRequest)] // invalid email
    [InlineData("", "", "", HttpStatusCode.BadRequest)] // empty
    public async Task Should_ReturnBadRequest_When_Input_Is_Invalid(
        string fullname, string email, string password, HttpStatusCode expectedStatusCode)
    {
        // Arrange
        var request = new SignUpCommand(fullname, email, password);

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/user/sign-up", request);
        var message = await response.Content.ReadAsStringAsync();

        // Assert
        _output.WriteLine($"Response message: {message}");
        response.StatusCode.Should().Be(expectedStatusCode);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_When_Password_Is_Too_Short()
    {
        // Arrange
        var request = new SignUpCommand("Johny Fullname", "test@test.com", "short");

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/user/sign-up", request);
        var message = await response.Content.ReadAsStringAsync();

        // Assert
        _output.WriteLine($"Response message: {message}");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_ReturnConflict_When_Email_Already_Exists()
    {
        // Arrange
        var existingUserRequest = new SignUpCommand("John Johny", "existing@test.com", "password12345678");
        await HttpClient.PostAsJsonAsync("api/user/sign-up", existingUserRequest);

        var newUserRequest = new SignUpCommand("New User", "existing@test.com", "password87654321");

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/user/sign-up", newUserRequest);
        var message = await response.Content.ReadAsStringAsync();

        // Assert
        _output.WriteLine($"Response message: {message}");
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}