using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Application.Visits.Commands.AddVisit;
internal sealed class AddVisitCommandHandler
    : ICommandHandler<AddVisitCommand,VisitDto>
{
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;
    private readonly IVisitRepository _visitRepository;
    private readonly IUnitOfWork _unitOfWork;
    public AddVisitCommandHandler(IUserContext userContext, IUserRepository userRepository, IVisitRepository visitRepository, IUnitOfWork unitOfWork)
    {
        _userContext = userContext;
        _userRepository = userRepository;
        _visitRepository = visitRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<VisitDto> Handle(AddVisitCommand request, CancellationToken cancellationToken)
    {
        UserId userId = _userContext.GetUserId;

        Address address = new(request.Street, request.City, request.PostalCode);
        DoctorName doctorName = request.DoctorName;
        VisitDescription description = request.VisitDescription;
        VisitType visitType = request.VisitType;
        Date date = request.Date;
        Date endDate = request.PredictedEndDate;

        Domain.Entities.Visit visit = Domain.Entities.Visit.Create(
            userId,
               address,
               date,
               doctorName,
               description,
               visitType,
               endDate);

        Domain.Entities.User? user = await _userRepository.GetUserWithVisitsAsync(userId, cancellationToken);

        if (user is null)
        {
             throw new UserNotFoundException();
        }

        user.AddVisit(visit);

        _visitRepository.Add(visit);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return visit.ToDto();
    }
}
