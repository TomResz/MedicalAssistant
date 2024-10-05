using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Attachment.Queries;
public sealed record GetAttachmentViewListQuery(
	Guid VisitId) : IRequest<IEnumerable<AttachmentViewDto>>;
