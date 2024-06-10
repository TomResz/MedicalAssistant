﻿using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Domain.Repositories;

namespace MedicalAssist.Application.User.Commands.RevokeRefreshToken;

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

        var user = await _userRepository.GetByIdAsync(userId,cancellationToken);

        if(user is null)
        {
            throw new UserNotFoundException();
        }

        user.RevokeRefreshToken();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}