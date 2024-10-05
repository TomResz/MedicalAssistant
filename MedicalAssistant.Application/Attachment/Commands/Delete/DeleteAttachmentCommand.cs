using MediatR;

namespace MedicalAssistant.Application.Attachment.Commands.Delete;
public sealed record DeleteAttachmentCommand(
	Guid Id) : IRequest;
