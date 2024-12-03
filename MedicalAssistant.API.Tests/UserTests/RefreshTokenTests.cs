using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MedicalAssistant.API.Tests.Abstractions;
using MedicalAssistant.Application.User.Commands.RefreshToken;
using Xunit.Abstractions;

namespace MedicalAssistant.API.Tests.UserTests;

public class RefreshTokenTests(TestWebAppFactory factory, ITestOutputHelper output, ITestOutputHelper helper) : BaseFunctionalTest(factory)
{
    private readonly ITestOutputHelper _helper = helper;
    [Fact]
    public async Task Should_Return_New_RefreshToken_When_Old_RefreshToken_Is_Not_Expired()
    {
        // arrange
        await InitializeUsersAsync();
        var (accessToken, refreshToken) = TokenHolder;
        var request = new RefreshTokenCommand(refreshToken, accessToken);
        
        // act
        var response = await HttpClient.PostAsJsonAsync("api/user/refresh-token", request);
        var newTokens = await response.Content.ReadFromJsonAsync<RefreshTokenResponse>();
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        newTokens.Should().NotBeNull();
        newTokens.AccessToken.Should().NotBeEmpty();
        newTokens.AccessToken.Should().NotBeSameAs(accessToken);
        newTokens.RefreshToken.Should().NotBeEmpty();
        newTokens.RefreshToken.Should().NotBeSameAs(refreshToken);
    }
    
    [Fact]
    public async Task Should_ReturnBadRequestStatus_When_Old_RefreshToken_Is_Not_Valid()
    {
        // arrange
        await InitializeUsersAsync();
        var (accessToken,refreshToken) = TokenHolder;
        var request = new RefreshTokenCommand(refreshToken + "123", accessToken);
        
        // act
        var response = await HttpClient.PostAsJsonAsync("api/user/refresh-token", request);
        var newTokens = await response.Content.ReadFromJsonAsync<RefreshTokenResponse>();
        
        // assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest,HttpStatusCode.InternalServerError);
    }
    
}