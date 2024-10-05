using MedicalAssistant.UI.Models.Attachment;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace MedicalAssistant.UI.Shared.Services.Attachment;

public sealed class AttachmentService : IAttachmentService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;

    public AttachmentService(
        HttpClient httpClient,
        IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

	public async Task<Response.Base.Response> Delete(Guid attachmentId)
	{
        var response = await _httpClient.DeleteAsync($"attachment/{attachmentId}");
        return await response.DeserializeResponse<Response.Base.Response>();
	}

	public async Task Download(Guid attachmentId)
    {
        var response = await _httpClient.GetAsync($"attachment/{attachmentId}");
       
        if (!response.IsSuccessStatusCode)
        {
            return;
        }

        var fileBytes = await response.Content.ReadAsByteArrayAsync();
        var fileName = response.Content.Headers.ContentDisposition?.FileName;

        await _jsRuntime.InvokeAsync<object>(
            "saveAsFile",
            fileName,
            Convert.ToBase64String(fileBytes));
    }
    public async Task<Response<List<AttachmentDto>>> GetListView(Guid visitId)
    {
        var response = await _httpClient.GetAsync($"{visitId}/attachment/");
        return await response.DeserializeResponse<List<AttachmentDto>>();
    }

    public async Task<Response<AttachmentDto>> Upload(IBrowserFile file,Guid visitId)
    {
        using var stream = file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024);
        using var streamContent = new StreamContent(stream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

        using var content = new MultipartFormDataContent
        {
            { streamContent, "file", file.Name }
        };

        var response = await _httpClient.PostAsync($"{visitId}/attachment", content);

        return await response.DeserializeResponse<AttachmentDto>();
    }
}
