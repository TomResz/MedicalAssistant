using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MedicalAssistant.API.Tests.Abstractions;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Visits.Commands.AddVisit;
using MedicalAssistant.Application.Visits.Commands.EditVisit;

namespace MedicalAssistant.API.Tests.VisitTests;

public class EditVisitTests(TestWebAppFactory applicationFactory)
    : BaseFunctionalTest(applicationFactory)
{
    [Fact]
    public async Task Should_ReturnNoContentStatus_When_VisitEdited()
    {
        // Create visit
        await InitializeUsersAsync($"johny{Guid.NewGuid()}@test.com");
        var createVistiRequest = new AddVisitCommand(
            "Opole",
            "11-111",
            "Opolska",
            "2024-11-30 12:00",
            "Dr John Smith",
            "Doctor",
            "Example description",
            "2024-11-30 12:30");
        var createVisitResponse = await HttpClient.PostAsJsonAsync("api/visit/add", createVistiRequest);
        var visitResponse = await createVisitResponse.Content.ReadFromJsonAsync<VisitDto>()!;

        var request = new EditVisitCommand(
            visitResponse!.VisitId,
            visitResponse.Address.City,
            visitResponse.Address.PostalCode,
            visitResponse.Address.Street,
            "2024-11-30 10:00",
            visitResponse.DoctorName,
            visitResponse.VisitDescription,
            "New description",
            "2024-11-30 10:30");

        var response = await HttpClient.PutAsJsonAsync("api/visit/edit", request);
        var responseContent = await response.Content.ReadFromJsonAsync<VisitDto>()!;

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().NotBeNull();
    }

    [Theory]
    [InlineData("", "11-111", "Opolska", "2024-11-30 12:00", "Dr John Smith",
        "Example description", "New description", "2024-11-30 12:30")] // Empty city
    [InlineData("Opole", "INVALID-ZIP", "Opolska", "2024-11-30 12:00", "Dr John Smith",
        "Example description", "New description", "2024-11-30 12:30")] // Invalid postal code
    [InlineData("Opole", "11-111", "", "2024-11-30 12:00", "Dr John Smith",
        "Example description", "New description", "2024-11-30 12:30")] // Empty street
    [InlineData("Opole", "11-111", "Opolska", "", "Dr John Smith",
        "Example description", "New description", "2024-11-30 12:30")] // Empty visit date
    [InlineData("Opole", "11-111", "Opolska", "2024-11-30 12:00", "",
        "Example description", "New description", "2024-11-30 12:30")] // Empty doctor name
    [InlineData("Opole", "11-111", "Opolska", "2024-11-30 12:00", "Dr John Smith",
        "", "New description", "2024-11-30 12:30")] // Empty old description
    [InlineData("Opole", "11-111", "Opolska", "2024-11-30 12:00", "Dr John Smith",
        "Example description", "", "2024-11-30 12:30")] // Empty new description
    public async Task Should_ReturnBadRequest_For_InvalidEditInputs(
        string city, string postalCode, string street, string visitDate, string doctorName, string oldDescription,
        string newDescription, string endDate)
    {
        // Create visit
        await InitializeUsersAsync($"johny{Guid.NewGuid()}@test.com");
        var createVisitRequest = new AddVisitCommand(
            "Opole",
            "11-111",
            "Opolska",
            "2024-11-30 12:00",
            "Dr John Smith",
            "Doctor",
            "Example description",
            "2024-11-30 12:30");
        var createVisitResponse = await HttpClient.PostAsJsonAsync("api/visit/add", createVisitRequest);
        var visitResponse = await createVisitResponse.Content.ReadFromJsonAsync<VisitDto>()!;

        // Attempt to edit with invalid data
        var request = new EditVisitCommand(
            visitResponse!.VisitId,
            city,
            postalCode,
            street,
            visitDate,
            doctorName,
            oldDescription,
            newDescription,
            endDate);

        var response = await HttpClient.PutAsJsonAsync("api/visit/edit", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}