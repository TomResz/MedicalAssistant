using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MedicalAssistant.API.Tests.Abstractions;
using MedicalAssistant.Application.MedicationRecommendations.Commands.AddRecommendation;
using MedicalAssistant.Application.Visits.Commands.AddVisit;
using MedicalAssistant.UI.Models.Visits;

namespace MedicalAssistant.API.Tests.MedicationTests;

public class AddMedicationRecommendationTests(TestWebAppFactory applicationFactory)
    : BaseFunctionalTest(applicationFactory)
{
    [Fact]
    public async Task Should_ReturnCratedStatus_When_MedicationRecommendationIsAdded()
    {
        // ARRANGE
        await InitializeUsersAsync($"andrew{Guid.NewGuid()}@gmail.com");
        string[] timeOfDays = ["morning","evening"];
        var addVisitCommand = new AddVisitCommand(
            "Opole",
            "11-111",
            "Opolska",
            "2024-11-30 12:00",
            "Dr John Smith",
            "Doctor",
            "Example description",
            "2024-11-30 12:30");

        var addVisitResponse = await HttpClient.PostAsJsonAsync("api/visit/add", addVisitCommand);
        var visitResponse = await addVisitResponse.Content.ReadFromJsonAsync<VisitDto>();
        
        var request = new AddMedicationRecommendationCommand(
            visitResponse!.Id,
            "note",
            "Abs",
            2,
            timeOfDays,
            DateTime.Now,
            DateTime.Now.AddDays(7));
        
        // ACT
        var response = await HttpClient.PostAsJsonAsync("api/recommendation/add", request);
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    
    
    [Fact]
    public async Task Should_ReturnCratedStatus_When_MedicationRecommendationIsAddedWithoutLinkedVisit()
    {
        // ARRANGE
        await InitializeUsersAsync($"andrew{Guid.NewGuid()}@gmail.com");
        string[] timeOfDays = ["morning","evening"];
        
        var request = new AddMedicationRecommendationCommand(
            null,
            "note",
            "Abs",
            2,
            timeOfDays,
            DateTime.Now,
            DateTime.Now.AddDays(7));
        
        // ACT
        var response = await HttpClient.PostAsJsonAsync("api/recommendation/add", request);
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    [Fact]
    public async Task Should_ReturnBadRequestStatus_When_VisitIsUnknown()
    {
        // ARRANGE
        await InitializeUsersAsync($"andrew{Guid.NewGuid()}@gmail.com");
        string[] timeOfDays = ["morning","evening"];
        
        var request = new AddMedicationRecommendationCommand(
            Guid.NewGuid(),
            "note",
            "Abs",
            2,
            timeOfDays,
            DateTime.Now,
            DateTime.Now.AddDays(7));
        
        // ACT
        var response = await HttpClient.PostAsJsonAsync("api/recommendation/add", request);
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}