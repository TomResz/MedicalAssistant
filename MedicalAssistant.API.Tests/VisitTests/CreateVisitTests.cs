using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MedicalAssistant.API.Tests.Abstractions;
using MedicalAssistant.Application.Visits.Commands.AddVisit;
using MedicalAssistant.UI.Models.Visits;

namespace MedicalAssistant.API.Tests.VisitTests;

public class CreateVisitTests(TestWebAppFactory applicationFactory) : BaseFunctionalTest(applicationFactory)
{
    [Fact]
    public async Task Should_ReturnCreatedStatus_When_Input_Data_Is_Valid()
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

        var response = await HttpClient.PostAsJsonAsync("api/visit/add", request);
        var visitResponse = await response.Content.ReadFromJsonAsync<VisitDto>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        visitResponse.Should().NotBeNull();
    }


    [Fact]
    public async Task Should_ReturnBadRequestStatus_When_PredictedDateIsSmallerThanStartDate()
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
            "2024-11-30 11:00");

        var response = await HttpClient.PostAsJsonAsync("api/visit/add", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task Should_ReturnBadRequest_When_RequiredFields_AreEmpty()
    {
        await InitializeUsersAsync($"johny{Guid.NewGuid()}@test.com");
        var request = new AddVisitCommand(
            "",
            "11-111",
            "",
            "",
            "",
            "Doctor",
            "",
            "2024-11-30 12:30");

        var response = await HttpClient.PostAsJsonAsync("api/visit/add", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("", "11-111", "Opolska", "2024-11-30 12:00", "Dr John Smith",
        "Doctor", "Example description", "2024-11-30 12:30")]
    [InlineData("Opole", "INVALID-ZIP", "Opolska", "2024-11-30 12:00", "Dr John Smith",
        "Doctor", "Example description", "2024-11-30 12:30")]
    [InlineData("Opole", "11-111", "", "2024-11-30 12:00", "Dr John Smith",
        "Doctor", "Example description", "2024-11-30 12:30")]
    [InlineData("Opole", "11-111", "Opolska", "", "Dr John Smith",
        "Doctor", "Example description", "2024-11-30 12:30")]
    [InlineData("Opole", "11-111", "Opolska", "2024-11-30 12:00", "",
        "Doctor", "Example description", "2024-11-30 12:30")]
    [InlineData("Opole", "11-111", "Opolska", "2024-11-30 12:00", "Dr John Smith",
        "Doctor", "", "2024-11-30 12:30")]
    public async Task Should_ReturnBadRequest_For_InvalidInputs(
        string city, string zipCode, string street, string date, string doctorName, string visitType,
        string description, string endTime)
    {
        await InitializeUsersAsync($"johny{Guid.NewGuid()}@test.com");
        var request = new AddVisitCommand(city, zipCode, street, date, doctorName,
            visitType, description, endTime);

        var response = await HttpClient.PostAsJsonAsync("api/visit/add", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}