using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MedicalAssistant.API.Models;
using MedicalAssistant.API.Tests.Abstractions;
using MedicalAssistant.Application.MedicalHistory.Commands.Add;

namespace MedicalAssistant.API.Tests.DiseaseHistoryTests;

public class DeleteDiseaseStageTests(TestWebAppFactory applicationFactory) : BaseFunctionalTest(applicationFactory)
{
    [Fact]
    public async Task Should_ReturnNoContentStatus_When_StageIsDeleted()
    {
        // ARRANGE
        await InitializeUsersAsync($"johny{Guid.NewGuid()}@johny.com");
        var addDiseaseRequest = new AddMedicalHistoryCommand(null, DateTime.Now, "Disease", "Example note", "Symptoms");
        var addResponse = await HttpClient.PostAsJsonAsync("api/medicalhistory", addDiseaseRequest);
        Guid diseaseId = await addResponse.Content.ReadFromJsonAsync<Guid>();

        var model = new AddDiseaseStageModel(null,
            "New name",
            "Note note",
            DateTime.Now.AddMinutes(50));
        var addStageResponse = await HttpClient.PostAsJsonAsync($"api/medicalhistory/{diseaseId}/stage", model);
        var id = await addStageResponse.Content.ReadFromJsonAsync<Guid>();
        
        // ACT
        var response = await HttpClient.DeleteAsync($"api/medicalhistory/{diseaseId}/stage/{id}");
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task Should_ReturnBadRequestStatus_When_UnknownStageId()
    {
        // ARRANGE
        await InitializeUsersAsync($"johny{Guid.NewGuid()}@johny.com");
        var addDiseaseRequest = new AddMedicalHistoryCommand(null, DateTime.Now, "Disease", "Example note", "Symptoms");
        var addResponse = await HttpClient.PostAsJsonAsync("api/medicalhistory", addDiseaseRequest);
        Guid diseaseId = await addResponse.Content.ReadFromJsonAsync<Guid>();
        
        
        // ACT
        var response = await HttpClient.DeleteAsync($"api/medicalhistory/{diseaseId}/stage/{Guid.NewGuid()}");
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    
    [Fact]
    public async Task Should_ReturnBadRequestStatus_When_UnknownDiseaseId()
    {
        // ARRANGE
        await InitializeUsersAsync($"johny{Guid.NewGuid()}@johny.com");
        var addDiseaseRequest = new AddMedicalHistoryCommand(null, DateTime.Now, "Disease", "Example note", "Symptoms");
        var addResponse = await HttpClient.PostAsJsonAsync("api/medicalhistory", addDiseaseRequest);
        Guid diseaseId = await addResponse.Content.ReadFromJsonAsync<Guid>();

        var model = new AddDiseaseStageModel(null,
            "New name",
            "Note note",
            DateTime.Now.AddMinutes(50));
        var addStageResponse = await HttpClient.PostAsJsonAsync($"api/medicalhistory/{diseaseId}/stage", model);
        var id = await addStageResponse.Content.ReadFromJsonAsync<Guid>();
        
        // ACT
        var response = await HttpClient.DeleteAsync($"api/medicalhistory/{Guid.NewGuid()}/stage/{id}");
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}