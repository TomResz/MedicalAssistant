using MedicalAssistant.UI.Models.Attachment;
using MedicalAssistant.UI.Shared.Response.Base;
using Microsoft.AspNetCore.Components.Forms;

namespace MedicalAssistant.UI.Shared.Services.Abstraction;

public interface IAttachmentService
{
	Task<Response<List<AttachmentDto>>> GetListView(Guid visitId);
	Task Download(Guid attachmentId);
	Task<Response<AttachmentDto>> Upload(IBrowserFile file,Guid visitId);

	Task<Response.Base.Response> Delete(Guid attachmentId);
}
