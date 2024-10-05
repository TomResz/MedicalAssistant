using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;
using Microsoft.AspNetCore.Http;

namespace MedicalAssistant.Application.Attachment.Commands.Add;

internal sealed class AddAttachmentCommandHandler
	: IRequestHandler<AddAttachmentCommand, AttachmentViewDto>
{
	private readonly IVisitRepository _visitRepository;
	private readonly IUnitOfWork _unitOfWork;

	public AddAttachmentCommandHandler(
		IVisitRepository visitRepository,
		IUnitOfWork unitOfWork)
	{
		_visitRepository = visitRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<AttachmentViewDto> Handle(AddAttachmentCommand request, CancellationToken cancellationToken)
	{
		var file = request.File;
		var visitId = request.VisitId;

		byte[] fileContent = await FileToStream(file);

		var attachment = Domain.Entites.Attachment.Create(
			visitId,
			file.FileName,
			fileContent);

		var visit = await _visitRepository.GetByIdAsync(visitId, cancellationToken);

		if (visit is null)
		{
			throw new UnknownVisitException(request.VisitId);
		}

		visit.AddAttachment(attachment);
		_visitRepository.Update(visit);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return attachment.ToViewDto();
	}

	private static async Task<byte[]> FileToStream(IFormFile file)
	{
		byte[] fileContent;
		using (var memoryStream = new MemoryStream())
		{
			await file.CopyToAsync(memoryStream);
			fileContent = memoryStream.ToArray();
		}

		return fileContent;
	}
}
