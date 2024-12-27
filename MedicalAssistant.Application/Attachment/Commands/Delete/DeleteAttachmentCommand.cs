using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.Attachment.Commands.Delete;
public sealed record DeleteAttachmentCommand(
	Guid Id) : ICommand;
