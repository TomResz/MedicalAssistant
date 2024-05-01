using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Domain.Abstraction;
using MedicalAssist.Domain.Repositories;

namespace MedicalAssist.Application.User.Commands.VerifyAccount;
internal sealed class VerifyAccountCommandHandler
	: IRequestHandler<VerifyAccountCommand>
{
	private readonly IUserRepository _userRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IClock _clock;
	public VerifyAccountCommandHandler(IUserRepository userRepository, IClock clock, IUnitOfWork unitOfWork)
	{
		_userRepository = userRepository;
		_clock = clock;
		_unitOfWork = unitOfWork;
	}

	public async Task Handle(VerifyAccountCommand request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByVerificationCodeAsync(request.CodeHash, cancellationToken);
		
		if(user is null)
		{
			throw new UnknownVerificationCode();
		}
		
		if(user.IsVerified)
		{
			return;
		}

		user.VerifyAccount(_clock.GetCurrentUtc());
		_userRepository.Update(user);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

	}
}
