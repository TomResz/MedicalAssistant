using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.Exceptions;
using MedicalAssist.Domain.Repositories;

namespace MedicalAssist.Application.Visits.Commands.ConfirmVisit;
internal sealed class ConfirmVisitCommandHandler : IRequestHandler<ConfirmVisitCommand>
{
    private readonly IVisitRepository _visitRepository;
    private readonly IUnitOfWork _unitOfWork;
    public ConfirmVisitCommandHandler(IVisitRepository visitRepository, IUnitOfWork unitOfWork)
    {
        _visitRepository = visitRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ConfirmVisitCommand request, CancellationToken cancellationToken)
    {
        Visit? visit = await _visitRepository.GetByIdAsync(request.VisistId, cancellationToken); 

        if(visit is null)
        {
            throw new UnknownVisitException(request.VisistId);
        }

        if (visit.WasVisited)
        {
            return;
        }

        visit.ConfirmVisit();
        _visitRepository.Update(visit);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
