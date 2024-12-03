using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MedicalAssistant.API.Tests.Abstractions;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Visits.Commands.AddVisit;

namespace MedicalAssistant.API.Tests.VisitTests;

public class DeleteVisitTests(TestWebAppFactory applicationFactory) : BaseFunctionalTest(applicationFactory)
{
    [Fact]
    public async Task Should_ReturnNoContentResponse_When_VisitDelete()
    {
        await InitializeUsersAsync($"johny{Guid.NewGuid()}@test.com");
        var request = new AddVisitCommand(
            "Opole",
            "11-111",
            "Opolska",
            "2024-11-30 12:00",
            "Dr John Smith",
            "Doctor",
            "Example description",
            "2024-11-30 12:30");

        var createVisitResponse = await HttpClient.PostAsJsonAsync("api/visit/add", request);
        var visitResponse = await createVisitResponse.Content.ReadFromJsonAsync<VisitDto>();
        
        var visitId = visitResponse!.VisitId;
        
        var response = await HttpClient.DeleteAsync($"api/visit/delete/{visitId}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task Should_ReturnBadRequestResponse_When_VisitNotExists()
    {
        await InitializeUsersAsync($"johny{Guid.NewGuid()}@test.com");
        
        var visitId = Guid.NewGuid();
        
        var response = await HttpClient.DeleteAsync($"api/visit/delete/{visitId}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}