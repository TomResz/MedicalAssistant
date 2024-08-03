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
	private readonly IUserLanguageContext _userLanguageContext;

	public RegenerateVerificationCodeCommandHandler(
		IUserRepository userRepository,
		ICodeVerification codeVerification,
		IClock clock,
		IUnitOfWork unitOfWork,
		IUserLanguageContext userLanguageContext)
	{
		_userRepository = userRepository;
		_codeVerification = codeVerification;
		_clock = clock;
		_unitOfWork = unitOfWork;
		_userLanguageContext = userLanguageContext;
	}

	public async Task Handle(RegenerateVerificationCodeCommand request, CancellationToken cancellationToken)
	{
		var language = _userLanguageContext.GetLanguage();
		var user = await _userRepository.GetByEmailWithUserVerificationAsync(request.Email);

        if (user is null)
        {
			throw new UserNotFoundException();
        }

		var code = _codeVerification.Generate(_clock.GetCurrentUtc());

		user.RegenerateVerificationCode(code, _clock.GetCurrentUtc(),language);
		
		_userRepository.Update(user);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
