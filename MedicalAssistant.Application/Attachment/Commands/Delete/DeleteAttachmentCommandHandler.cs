using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects.IDs;
using System.Linq.Expressions;

namespace MedicalAssistant.Application.Attachment.Commands.Delete;
internal sealed class DeleteAttachmentCommandHandler
	: IRequestHandler<DeleteAttachmentCommand>
{
	private readonly IAttachmentRepository _attachmentRepository;
	private readonly IUnitOfWork _unitOfWork;
	public DeleteAttachmentCommandHandler(
		IAttachmentRepository attachmentRepository,
		IUnitOfWork unitOfWork)
	{
		_attachmentRepository = attachmentRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
	{
		var attachmentId = new AttachmentId(request.Id);
		Expression<Func<Domain.Entities.Attachment, bool>> predicate = (x) => x.Id == attachmentId;
		
		var wasDeleted = await _attachmentRepository.DeleteAsync(predicate) > 0;

		if (!wasDeleted)
		{
			throw new UnknownAttachmentException(request.Id);
		}

		await _unitOfWork.SaveChangesAsync(cancellationToken);
	}
}
