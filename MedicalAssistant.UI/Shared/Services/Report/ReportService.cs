using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.JSInterop;

namespace MedicalAssistant.UI.Shared.Services.Report;

public class ReportService : IReportService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;

    public ReportService(
        HttpClient httpClient,
        IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> DownloadVisitReport(List<Guid> visitIds)
    {
        var queryParameters = string.Join("&", visitIds.Select(id => $"id={id}"));
        var route = $"report/visit?{queryParameters}";
        var response = await _httpClient.GetAsync(route);
        
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        await DeserializeFile(response);
        return true;
    }

    public async Task<bool> DownloadMedicalHistoryReport(List<Guid> ids)
    {
        var queryParameters = string.Join("&", ids.Select(id => $"id={id}"));
        var route = $"report/medical-history?{queryParameters}";
        var response = await _httpClient.GetAsync(route);
        
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        await DeserializeFile(response);
        return true;
    }

    public async Task<bool> DownloadNoteReport(List<Guid> ids)
    {
        var queryParameters = string.Join("&", ids.Select(id => $"id={id}"));
        var route = $"report/notes?{queryParameters}";
        var response = await _httpClient.GetAsync(route);
        
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        await DeserializeFile(response);
        return true;
    }

    public async Task<bool> DownloadMedicationReport(List<Guid> ids)
    {
        var queryParameters = string.Join("&", ids.Select(id => $"id={id}"));
        var route = $"report/medications?{queryParameters}";
        var response = await _httpClient.GetAsync(route);
        
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        await DeserializeFile(response);
        return true;
    }
    

    private async Task DeserializeFile(HttpResponseMessage response)
    {
        var fileBytes = await response.Content.ReadAsByteArrayAsync();
        var fileName = response.Content.Headers.ContentDisposition?.FileName;

        await _jsRuntime.InvokeAsync<object>(
            "saveAsFile",
            fileName,
            Convert.ToBase64String(fileBytes));
    }
}