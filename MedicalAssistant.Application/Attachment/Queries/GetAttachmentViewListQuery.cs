using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Attachment.Queries;
public sealed record GetAttachmentViewListQuery(
	Guid VisitId) : IQuery<IEnumerable<AttachmentViewDto>>;
