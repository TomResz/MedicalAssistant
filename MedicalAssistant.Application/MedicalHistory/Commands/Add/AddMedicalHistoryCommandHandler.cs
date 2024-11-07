using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.MedicalHistory.Commands.Add;

internal sealed class AddMedicalHistoryCommandHandler
    : IRequestHandler<AddMedicalHistoryCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVisitRepository _visitRepository;
    private readonly IUserContext _userContext;
    private readonly IMedicalHistoryRepository _medicalHistoryRepository;
    
    public AddMedicalHistoryCommandHandler(
        IUnitOfWork unitOfWork,
        IVisitRepository visitRepository,
        IUserContext userContext,
        IMedicalHistoryRepository medicalHistoryRepository)
    {
        _unitOfWork = unitOfWork;
        _visitRepository = visitRepository;
        _userContext = userContext;
        _medicalHistoryRepository = medicalHistoryRepository;
    }

    public async Task<Guid> Handle(AddMedicalHistoryCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetUserId;

        var medicalHistory = Domain.Entities.MedicalHistory.Create(
            userId,
            request.StartDate,
            null,
            request.Name,
            request.Notes,
            request.SymptomDescription,
            request.VisitId);

        if (request.VisitId is not null)
        {
            var visit = await _visitRepository.GetByIdAsync(request.VisitId, cancellationToken);

            if (visit is null)
            {
                throw new UnknownVisitException();
            }

            visit.AddMedicalHistory(medicalHistory);
        }
        else
        {
            _medicalHistoryRepository.Add(medicalHistory);
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return medicalHistory.Id;
    }
}