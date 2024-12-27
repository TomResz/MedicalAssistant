using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Attachment.Queries;
public sealed record DownloadAttachmentByIdQuery(
    Guid Id) : IQuery<AttachmentDto?>;
