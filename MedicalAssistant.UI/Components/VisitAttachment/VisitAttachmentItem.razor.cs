using MedicalAssistant.UI.Models.Attachment;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.VisitAttachment;

public partial class VisitAttachmentItem
{
	[Parameter]
	public AttachmentDto Attachment { get; set; }

	[Parameter]
	public EventCallback<Guid> OnAttachmentDeleted { get; set; }

	[Inject]

	public IAttachmentService AttachmentService { get; set; }


	private MudMessageBox _mudMessageBox;

	private async Task Delete(Guid attachmentId)
	{
		var dialogResult = await _mudMessageBox.ShowAsync();
		if (dialogResult is null || dialogResult is false)
		{
			return;
		}
		var response = await AttachmentService.Delete(attachmentId);

		if (response.IsSuccess)
		{
			await OnAttachmentDeleted.InvokeAsync(attachmentId);
		}
	}

	private async Task Download(Guid attachmentId)
	{
		await AttachmentService.Download(attachmentId);
	}
}
