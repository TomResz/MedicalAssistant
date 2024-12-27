using MediatR;
using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.User.Commands.VerifyAccount;
internal sealed class VerifyAccountCommandHandler
	: ICommandHandler<VerifyAccountCommand>
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
