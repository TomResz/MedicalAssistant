using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Application.Security;
using MedicalAssist.Domain.Abstraction;
using MedicalAssist.Domain.Repositories;

namespace MedicalAssist.Application.User.Commands.RegenerateVerificationCode;
internal sealed class RegenerateVerificationCodeCommandHandler
	: IRequestHandler<RegenerateVerificationCodeCommand>
{
	private readonly IUserRepository _userRepository;
	private readonly ICodeVerification _codeVerification;
	private readonly IClock _clock;
	private readonly IUnitOfWork _unitOfWork;
	public RegenerateVerificationCodeCommandHandler(
		IUserRepository userRepository,
		ICodeVerification codeVerification,
		IClock clock,
		IUnitOfWork unitOfWork)
	{
		_userRepository = userRepository;
		_codeVerification = codeVerification;
		_clock = clock;
		_unitOfWork = unitOfWork;
	}

	public async Task Handle(RegenerateVerificationCodeCommand request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByEmailWithUserVerificationAsync(request.Email);

        if (user is null)
        {
			throw new UserNotFoundException();
        }

		var code = _codeVerification.Generate(_clock.GetCurrentUtc());

		user.RegenerateVerificationCode(code, _clock.GetCurrentUtc());
		
		_userRepository.Update(user);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
