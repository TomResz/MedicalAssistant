using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using MedicalAssistant.API.Tests.Abstractions;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Visits.Commands.AddVisit;

namespace MedicalAssistant.API.Tests.AttachmentTests;

public class AddAttachmentTests(TestWebAppFactory applicationFactory) : BaseFunctionalTest(applicationFactory)
{
    [Fact]
    public async Task Should_ReturnCreatedStatus_When_Attachment_IsAdded()
    {
        // Arrange
        var visitId = await InitializeTest();

        TestFile[] testFiles =
        [
            new TestFile("test.pdf", "application/pdf", 200 * 1024),
            new TestFile("test.jpg", "image/jpeg", 200 * 1024),
            new TestFile("test.png", "image/png", 200 * 1024)
        ];

        foreach (var testFile in testFiles)
        {
            using var content = new MultipartFormDataContent();
            using var fileStream = GenerateFileStream(testFile.Size);
            var fileContentStream = new StreamContent(fileStream);
            fileContentStream.Headers.ContentType = new MediaTypeHeaderValue(testFile.ContentType);

            content.Add(fileContentStream, "file", testFile.FileName);

            // Act
            var response = await HttpClient.PostAsync($"api/{visitId}/attachment", content);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            var responseContent = await response.Content.ReadFromJsonAsync<AttachmentResponse>();

            responseContent.Should().NotBeNull();
            responseContent.Id.Should().NotBeEmpty();
            response.Headers.Location.Should().Be($"/api/attachment/{responseContent.Id}");
        }
    }

    [Fact]
    public async Task Should_ReturnBadRequest_When_AttachmentSizeExceeded()
    {
        // Arrange
        var visitId = await InitializeTest();

        
        TestFile[] testFiles =
        [
            new("test.pdf", "application/pdf", 15 * 1024 * 1024 ), // 15 MB
            new("test.jpg", "image/jpeg", 12 * 1024 * 1024), // 12 MB
            new("test.png", "image/png", 13 * 1024 * 1024) // 13 MB
        ];

        foreach (var testFile in testFiles)
        {
            using var content = new MultipartFormDataContent();
            using var fileStream = GenerateFileStream(testFile.Size);
            var fileContentStream = new StreamContent(fileStream);
            fileContentStream.Headers.ContentType = new MediaTypeHeaderValue(testFile.ContentType);

            content.Add(fileContentStream, "file", testFile.FileName);

            // Act
            var response = await HttpClient.PostAsync($"api/{visitId}/attachment", content);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
    [Fact]
    public async Task Should_ReturnBadRequest_When_AttachmentTypeIsInvalid()
    {
        // Arrange
        var visitId = await InitializeTest();


        TestFile[] testFiles =
        [
            new("excel.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 9 * 1024 ), 
            new("zip.zip", "application/zip", 25 * 1024 ), 
            new("mp4.mp4", "video/mp4", 30 * 1024 )       
        ];

        foreach (var testFile in testFiles)
        {
            using var content = new MultipartFormDataContent();
            using var fileStream = GenerateFileStream(testFile.Size);
            var fileContentStream = new StreamContent(fileStream);
            fileContentStream.Headers.ContentType = new MediaTypeHeaderValue(testFile.ContentType);

            content.Add(fileContentStream, "file", testFile.FileName);

            // Act
            var response = await HttpClient.PostAsync($"api/{visitId}/attachment", content);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
    private async Task<Guid> InitializeTest()
    {
        await InitializeUsersAsync($"johny{Guid.NewGuid()}@gmail.com");
        var request = new AddVisitCommand(
            "Opole",
            "11-111",
            "Opolska",
            "2024-11-30 12:00",
            "Dr John Smith",
            "Doctor",
            "Example description",
            "2024-11-30 12:30");

        var visitResponse = await HttpClient.PostAsJsonAsync("api/visit/add", request);
        var visitId = (await visitResponse.Content.ReadFromJsonAsync<VisitDto>())!.VisitId;
        return visitId;
    }


    private MemoryStream GenerateFileStream(int sizeInBytes)
    {
        var randomData = new byte[sizeInBytes];
        new Random().NextBytes(randomData);
        return new MemoryStream(randomData);
    }
}

file class TestFile(string fileName, string contentType, int size)
{
    public string FileName { get; } = fileName;
    public string ContentType { get; } = contentType;
    public int Size { get; } = size;
}

file class AttachmentResponse
{
    public Guid Id { get; set; }
}