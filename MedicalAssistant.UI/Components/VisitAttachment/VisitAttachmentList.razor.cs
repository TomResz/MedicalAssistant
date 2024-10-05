using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MedicalAssistant.UI.Models.Attachment;
using MudBlazor;
using MedicalAssistant.UI.Shared.Services.Attachment;
using MedicalAssistant.UI.Shared.Resources;

namespace MedicalAssistant.UI.Components.VisitAttachment;

public partial class VisitAttachmentList
{
	[Parameter]
	public Guid VisitId { get; set; }

	private bool _loading = true;
	private List<AttachmentDto> _attachments;

	[Inject]
	public IAttachmentService AttachmentService { get; set; }

	[Inject]
	public ISnackbar Snackbar { get; set; }

	protected override async Task OnInitializedAsync()
	{
		var response = await AttachmentService.GetListView(VisitId);
		if (response.IsSuccess)
		{
			_attachments = [.. response.Value];
		}
		else
		{
			_attachments = [];
		}
		_loading = false;
	}

	public async Task UploadFile(IBrowserFile file)
	{
		if (file is null || file.Size == 0)
		{
			Console.WriteLine("Empty file");
			return;
		}

		if (file.Size > 1024 * 1024 * 10) // 10 MB
		{
			Snackbar.Add(Translations.FileMaximumSize,Severity.Error);
			return;
		}
		var response = await AttachmentService.Upload(file, VisitId);

		if (response.IsSuccess)
		{
			var attachment = response.Value!;
			_attachments.Add(attachment);
			StateHasChanged();
		}
		else
		{
			var feedbackMessage = AttachmentEndpointErrors.MatchErrors(response.ErrorDetails!.Type);
			Snackbar.Add(feedbackMessage,Severity.Error);
		}
	}

	public void Delete(Guid attachmentId)
	{
		var attachment = _attachments
			.Where(x => x.Id == attachmentId)
			.First();
		_attachments.Remove(attachment);
		StateHasChanged();
	}

}
