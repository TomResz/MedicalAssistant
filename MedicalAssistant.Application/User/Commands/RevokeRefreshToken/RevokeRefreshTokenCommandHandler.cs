using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.User.Commands.RevokeRefreshToken;

internal sealed class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand>
{
    private readonly IUserContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RevokeRefreshTokenCommandHandler(IUserContext context, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _context = context;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var userId = _context.GetUserId;

        var user = await _userRepository.GetWithRefreshTokens(userId,cancellationToken);

        if(user is null)
        {
            throw new UserNotFoundException();
        }

        user.RemoveRefreshToken(request.RefreshToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}