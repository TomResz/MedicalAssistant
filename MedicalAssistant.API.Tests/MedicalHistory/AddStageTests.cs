using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MedicalAssistant.API.Models;
using MedicalAssistant.API.Tests.Abstractions;
using MedicalAssistant.Application.MedicalHistory.Commands.Add;

namespace MedicalAssistant.API.Tests.MedicalHistory;

public class AddStageTests(TestWebAppFactory applicationFactory) : BaseFunctionalTest(applicationFactory)
{
    [Fact]
    public async Task Should_ReturnCreatedStatus_When_StageIsAdded()
    {
        var id = await InitializeData();
        
        var model = new AddDiseaseStageModel(
            null,
            "note",
            "Name",
            DateTime.Now);
        
        var request = await HttpClient.PostAsJsonAsync($"api/medicalhistory/{id}/stage", model);
        var returnedId = await request.Content.ReadFromJsonAsync<Guid>();

        request.StatusCode.Should().Be(HttpStatusCode.Created);
        returnedId.Should().NotBe(Guid.Empty);
    }

    
    [Fact]
    public async Task Should_ReturnBadRequestStatus_When_DateIsInvalid()
    {
        var id = await InitializeData();
        
        var model = new AddDiseaseStageModel(
            null,
            "Note",
            "Name",
            DateTime.Now.AddDays(-10));
        
        var request = await HttpClient.PostAsJsonAsync($"api/medicalhistory/{id}/stage", model);

        request.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    
    [Fact]
    public async Task Should_ReturnBadRequestStatus_When_NameIsEmpty()
    {
        var id = await InitializeData();
        
        var model = new AddDiseaseStageModel(
            null,
            "Note",
            string.Empty,
            DateTime.Now);
        
        var request = await HttpClient.PostAsJsonAsync($"api/medicalhistory/{id}/stage", model);

        request.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    
    [Fact]
    public async Task Should_ReturnBadRequestStatus_When_MedicalHistoryIdIsInvalid()
    {
        var id = await InitializeData();
        
        var model = new AddDiseaseStageModel(
            null,
            "Note",
            "Name",
            DateTime.Now);
        
        var request = await HttpClient.PostAsJsonAsync($"api/medicalhistory/{Guid.NewGuid()}/stage", model);

        request.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    private async Task<Guid> InitializeData()
    {
        await InitializeUsersAsync($"john{Guid.NewGuid()}@john.com");
        
        var command = new AddMedicalHistoryCommand(
            null,
            DateTime.Now,
            "Example name",
            "Note",
            "symptons");
        
        // ACT
        var response = await HttpClient.PostAsJsonAsync("api/medicalhistory", command);
       return await response.Content.ReadFromJsonAsync<Guid>();
    }
}