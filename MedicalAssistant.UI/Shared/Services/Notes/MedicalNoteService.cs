﻿using MedicalAssistant.UI.Models.MedicalNotes;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using System.Net.Http.Json;

namespace MedicalAssistant.UI.Shared.Services.Notes;

public class MedicalNoteService(HttpClient httpClient) : IMedicalNoteService
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

    public async Task<Response.Base.Response> Edit(EditNoteRequest request)
    {
        var response = await _httpClient.PatchAsJsonAsync("medicalnote/",request);
        return await response.DeserializeResponse();
    }

    public async Task<Response.Base.Response> Delete(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"medicalnote/{id}");
        return await response.DeserializeResponse();
    }

    public async Task<Response<List<NoteDto>>> GetCurrents(DateTime currentDate)
    {
        var response = await _httpClient.GetAsync($"medicalnote/{currentDate:yyyy-MM-dd}");
        return await response.DeserializeResponse<List<NoteDto>>();
    }
}