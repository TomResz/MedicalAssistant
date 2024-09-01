using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Dto;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Domain.ComplexTypes;
using MedicalAssist.Domain.Repositories;
using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Application.Visits.Commands.AddVisit;
internal sealed class AddVisitCommandHandler
    : IRequestHandler<AddVisitCommand,VisitDto>
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

        Domain.Entites.Visit visit = Domain.Entites.Visit.Create(
            userId,
               address,
               date,
               doctorName,
               description,
               visitType,
               endDate);

        Domain.Entites.User? user = await _userRepository.GetUserWithVisitsAsync(userId, cancellationToken);

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
