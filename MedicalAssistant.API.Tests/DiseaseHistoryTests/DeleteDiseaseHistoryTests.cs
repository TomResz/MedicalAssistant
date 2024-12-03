using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MedicalAssistant.API.Tests.Abstractions;
using MedicalAssistant.Application.MedicalHistory.Commands.Add;

namespace MedicalAssistant.API.Tests.DiseaseHistoryTests;

public class DeleteDiseaseHistoryTests(TestWebAppFactory applicationFactory) : BaseFunctionalTest(applicationFactory)
{
    [Fact]
    public async Task Should_ReturnNoContentStatus_When_DiseaseDeleted()
    {
        // ARRANGE
        await InitializeUsersAsync($"johny{Guid.NewGuid()}@johny.com");
        var addMedicalHistoryCommand = new AddMedicalHistoryCommand(null, DateTime.Now, "Disease", "Example note", "Symptoms");
        var addResponse = await HttpClient.PostAsJsonAsync("api/medicalhistory", addMedicalHistoryCommand);
        var id = await addResponse.Content.ReadFromJsonAsync<Guid>();
        
        // ACT
        var response = await HttpClient.DeleteAsync($"api/medicalhistory/{id}");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task Should_ReturnBadRequestStatus_When_DiseaseNotExists()
    {
        // ARRANGE
        await InitializeUsersAsync($"johny{Guid.NewGuid()}@johny.com");
        
        // ACT
        var response = await HttpClient.DeleteAsync($"api/medicalhistory/{Guid.NewGuid()}");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}