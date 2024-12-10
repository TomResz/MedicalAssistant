using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MedicalAssistant.API.Tests.Abstractions;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.MedicalHistory.Commands.Add;
using MedicalAssistant.Application.Visits.Commands.AddVisit;

namespace MedicalAssistant.API.Tests.MedicalHistory;

public class MedicalHistoryTests(TestWebAppFactory applicationFactory) : BaseFunctionalTest(applicationFactory)
{
    [Fact]
    public async Task Should_ReturnCreatedStatus_When_History_Is_Added()
    {
        // ARRANGE
        await InitializeUsersAsync($"john{Guid.NewGuid()}@email.com");

        var command = new AddMedicalHistoryCommand(
            null,
            DateTime.Now,
            "Example name",
            null,
            null);
        
        // ACT
        var response = await HttpClient.PostAsJsonAsync("api/medicalhistory", command);
        Guid id = await response.Content.ReadFromJsonAsync<Guid>();

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        id.Should().NotBe(Guid.Empty);
    }
    
    [Fact]
    public async Task Should_ReturnCreatedStatus_When_HistoryIsAddedWithLinkedVisit()
    {
        // ARRANGE
        await InitializeUsersAsync($"john{Guid.NewGuid()}@email.com");
        var request = new AddVisitCommand(
            "Opole",
            "11-111",
            "Opolska",
            "2024-11-30 12:00",
            "Dr John Smith",
            "Doctor",
            "Example description",
            "2024-11-30 12:30");
        var firstResponse = await HttpClient.PostAsJsonAsync("api/visit/add", request);
        var visitResponse = await firstResponse.Content.ReadFromJsonAsync<VisitDto>();
        
        var command = new AddMedicalHistoryCommand(
            visitResponse!.VisitId,
            DateTime.Now,
            "Example name",
            null,
            null);
        
        // ACT
        var response = await HttpClient.PostAsJsonAsync("api/medicalhistory", command);
        Guid id = await response.Content.ReadFromJsonAsync<Guid>();

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        id.Should().NotBe(Guid.Empty);
    }
    
    
    [Fact]
    public async Task Should_ReturnBadRequestStatus_When_VisitIsUnknown()
    {
        // ARRANGE
        await InitializeUsersAsync($"john{Guid.NewGuid()}@email.com");
        
        var command = new AddMedicalHistoryCommand(
            Guid.NewGuid(),
            DateTime.Now,
            "Example name",
            null,
            null);
        
        // ACT
        var response = await HttpClient.PostAsJsonAsync("api/medicalhistory", command);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}