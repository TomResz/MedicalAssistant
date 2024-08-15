using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Domain.Exceptions;
using MedicalAssist.Domain.Repositories;

namespace MedicalAssist.Application.Visits.Commands.DeleteVisit;
internal sealed class DeleteVisitCommandHandler
	: IRequestHandler<DeleteVisitCommand>
{
	private readonly IVisitRepository _visitRepository;
	private readonly IUnitOfWork _unitOfWork;


	public DeleteVisitCommandHandler(
		IUnitOfWork unitOfWork, 
		IVisitRepository visitRepository)
	{
		_visitRepository = visitRepository;
		_unitOfWork = unitOfWork;	
	}

	public async Task Handle(DeleteVisitCommand request, CancellationToken cancellationToken)
	{
		var visit = await _visitRepository.GetByIdAsync(request.VisitId, cancellationToken);
		
		if(visit is null)
		{
			throw new UnknownVisitException(request.VisitId);
		}
		_visitRepository.Remove(visit);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
