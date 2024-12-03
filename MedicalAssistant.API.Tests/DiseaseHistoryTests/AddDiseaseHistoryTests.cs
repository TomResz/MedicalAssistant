using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MedicalAssistant.API.Tests.Abstractions;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.MedicalHistory.Commands.Add;
using MedicalAssistant.Application.Visits.Commands.AddVisit;

namespace MedicalAssistant.API.Tests.DiseaseHistoryTests;

public class AddDiseaseHistoryTests(TestWebAppFactory applicationFactory)
    : BaseFunctionalTest(applicationFactory)
{
    [Fact]
    public async Task Should_ReturnCreatedStatus_When_InputIsValid()
    {
        // ARRANGE
        await InitializeUsersAsync($"johny{Guid.NewGuid()}@johny.com");
        var request = new AddMedicalHistoryCommand(null, DateTime.Now, "Disease", "Example note", "Symptoms");
        
        // ACT
        var response = await HttpClient.PostAsJsonAsync("api/medicalhistory", request);
        Guid responseContent = await response.Content.ReadFromJsonAsync<Guid>();
        
        // ASSERT
        responseContent.Should().NotBe(Guid.Empty);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    
    [Fact]
    public async Task Should_ReturnCreatedStatus_When_InputIsValidAndKnownVisit()
    {
        // ARRANGE
        await InitializeUsersAsync($"johny{Guid.NewGuid()}@johny.com");
        var addVisitRequest = new AddVisitCommand(
            "Opole",
            "11-111",
            "Opolska",
            "2024-11-30 12:00",
            "Dr John Smith",
            "Doctor",
            "Example description",
            "2024-11-30 12:30");

        var visitResponse = await HttpClient.PostAsJsonAsync("api/visit/add", addVisitRequest);
        var visitDto  = await visitResponse.Content.ReadFromJsonAsync<VisitDto>();
        
        var request = new AddMedicalHistoryCommand(visitDto!.VisitId, DateTime.Now, "Disease", "Example note", "Symptoms");
        
        // ACT
        var response = await HttpClient.PostAsJsonAsync("api/medicalhistory", request);
        Guid responseContent = await response.Content.ReadFromJsonAsync<Guid>();
        
        // ASSERT
        responseContent.Should().NotBe(Guid.Empty);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    
    [Fact]
    public async Task Should_ReturnBadRequestStatus_When_VisitIdIsNotEmptyAndUnknownVisit()
    {
        // ARRANGE
        await InitializeUsersAsync($"johny{Guid.NewGuid()}@johny.com");
        
        var request = new AddMedicalHistoryCommand(Guid.NewGuid(), DateTime.Now, "Disease", "Example note", "Symptoms");
        
        // ACT
        var response = await HttpClient.PostAsJsonAsync("api/medicalhistory", request);
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Should_ReturnBadRequestStatus_When_InputDataIsInvalid()
    {
        // ARRANGE
        await InitializeUsersAsync($"johny{Guid.NewGuid()}@johny.com");
        
        var request = new AddMedicalHistoryCommand(null, DateTime.Now, string.Empty, "Notes", "symptoms");

        // ACT
        var response = await HttpClient.PostAsJsonAsync("api/medicalhistory", request);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}