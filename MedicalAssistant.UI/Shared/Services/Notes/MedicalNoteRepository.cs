using System.Net.Http.Json;
using MedicalAssistant.UI.Components.MedicalNote;
using MedicalAssistant.UI.Models.MedicalNotes;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;

namespace MedicalAssistant.UI.Shared.Services.Notes;

public class MedicalNoteRepository(HttpClient httpClient) : IMedicalNoteRepository
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<Response.Base.Response<IEnumerable<TagDto>>> GetTags()
    {
        var response = await _httpClient.GetAsync("medicalnote/tags");
        return await response.DeserializeResponse<IEnumerable<TagDto>>();
    }

    public async Task<Response<List<NoteDto>>> GetNotes()
    {
        var response = await _httpClient.GetAsync("medicalnote/");
        return await response.DeserializeResponse<List<NoteDto>>();
    }

    public async Task<Response<List<NoteDto>>> GetBySearchTermAndTags(string? searchTerm, string[]? tags)
    {
        string route = "medicalnote/search";

        var queryParams = new List<string>();
        
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            queryParams.Add($"term={Uri.EscapeDataString(searchTerm)}");
        }
        if (tags is not null && tags.Length > 0)
        {
            queryParams.AddRange(
                tags
                    .Select(tag => $"tag={Uri.EscapeDataString(tag)}"));
        }
        
        if (queryParams.Count > 0)
        {
            route += "?" + string.Join("&", queryParams);
        }
        var response = await _httpClient.GetAsync(route);
        return await response.DeserializeResponse<List<NoteDto>>();
    }

    public async Task<Response<Guid>> Add(AddNoteRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("medicalnote/",request);
        return await response.DeserializeResponse<Guid>();
    }
}