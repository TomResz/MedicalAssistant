using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.User.Commands.ReactivateAccount;

public sealed record ReactivateAccountCommand() : IRequest;

internal sealed class DeactivateAccountCommandHandler : IRequestHandler<ReactivateAccountCommand>
{
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClock _clock;

    public DeactivateAccountCommandHandler(
        IUserContext userContext,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IClock clock)
    {
        _userContext = userContext;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _clock = clock;
    }

    public async Task Handle(ReactivateAccountCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetUserId;

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken,true);

        if (user is null)
        {
            throw new UserNotFoundException();
        }

        var currentDate = _clock.GetCurrentUtc();
        user.ReactivateAccount();
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
    
}