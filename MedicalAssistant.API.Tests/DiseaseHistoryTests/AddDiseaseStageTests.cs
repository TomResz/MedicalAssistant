using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MedicalAssistant.API.Models;
using MedicalAssistant.API.Tests.Abstractions;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.MedicalHistory.Commands.Add;
using MedicalAssistant.Application.Visits.Commands.AddVisit;

namespace MedicalAssistant.API.Tests.DiseaseHistoryTests;

public class AddDiseaseStageTests(TestWebAppFactory applicationFactory) 
    : BaseFunctionalTest(applicationFactory)
{
    [Fact]
    public async Task Should_ReturnCreatedStatus_When_StageIsAdded()
    {
        await InitializeUsersAsync($"johny{Guid.NewGuid()}@johny.com");
        var addDiseaseRequest = new AddMedicalHistoryCommand(null, DateTime.Now, "Disease", "Example note", "Symptoms");
        var addResponse = await HttpClient.PostAsJsonAsync("api/medicalhistory", addDiseaseRequest);
        Guid responseContent = await addResponse.Content.ReadFromJsonAsync<Guid>();

        var model = new AddDiseaseStageModel(null,
        "New name",
            "Note note",
            DateTime.Now.AddMinutes(50));
        
        var response = await HttpClient.PostAsJsonAsync($"api/medicalhistory/{responseContent}/stage", model);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    [Fact]
    public async Task Should_ReturnCreatedStatus_When_StageIsAddedWithVisit()
    {
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
        var addDiseaseRequest = new AddMedicalHistoryCommand(null, DateTime.Now, "Disease", "Example note", "Symptoms");
        var addResponse = await HttpClient.PostAsJsonAsync("api/medicalhistory", addDiseaseRequest);
        Guid responseContent = await addResponse.Content.ReadFromJsonAsync<Guid>();
        
        var model = new AddDiseaseStageModel(
            visitDto!.VisitId,
            "New name",
            "Note note",
            DateTime.Now.AddMinutes(50));
        
        var response = await HttpClient.PostAsJsonAsync($"api/medicalhistory/{responseContent}/stage", model);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    
    [Fact]
    public async Task Should_ReturnBadRequestStatus_When_DiseaseIdIsUnknown()
    {
        await InitializeUsersAsync($"johny{Guid.NewGuid()}@johny.com");

        var model = new AddDiseaseStageModel(null,
            "New name",
            "Note note",
            DateTime.Now.AddMinutes(50));
        
        var response = await HttpClient.PostAsJsonAsync($"api/medicalhistory/{Guid.NewGuid()}/stage", model);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Should_ReturnBadRequestStatus_When_EmptyName()
    {
        await InitializeUsersAsync($"johny{Guid.NewGuid()}@johny.com");

        var model = new AddDiseaseStageModel(null,
            "New name",
            string.Empty,
            DateTime.Now.AddMinutes(50));
        
        var response = await HttpClient.PostAsJsonAsync($"api/medicalhistory/{Guid.NewGuid()}/stage", model);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Should_ReturnBadRequestStatus_When_DateIsGreaterThanDiseaseDate()
    {
        await InitializeUsersAsync($"johny{Guid.NewGuid()}@johny.com");
        var addDiseaseRequest = new AddMedicalHistoryCommand(null, DateTime.Now, "Disease", "Example note", "Symptoms");
        var addResponse = await HttpClient.PostAsJsonAsync("api/medicalhistory", addDiseaseRequest);
        Guid responseContent = await addResponse.Content.ReadFromJsonAsync<Guid>();

        var model = new AddDiseaseStageModel(
            null,
            "New name",
            "Note note",
            DateTime.Now.AddHours(-50));
        
        var response = await HttpClient.PostAsJsonAsync($"api/medicalhistory/{responseContent}/stage", model);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}