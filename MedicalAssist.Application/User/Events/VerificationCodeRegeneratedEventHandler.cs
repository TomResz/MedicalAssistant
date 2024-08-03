using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Domain.Events;
using MedicalAssist.Domain.Repositories;

namespace MedicalAssist.Application.User.Events;

internal sealed class VerificationCodeRegeneratedEventHandler : INotificationHandler<VerificationCodeRegeneratedEvent>
{
	private readonly IEmailService _emailService;
	private readonly IUserRepository _userRepository;
	public VerificationCodeRegeneratedEventHandler(IEmailService emailService, IUserRepository userRepository)
	{
		_emailService = emailService;
		_userRepository = userRepository;
	}

	public async Task Handle(VerificationCodeRegeneratedEvent notification, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByIdWithUserVerificationAsync(notification.UserId, cancellationToken);

		if(user is null)
		{
			throw new UserNotFoundException();
		}

		await _emailService.SendMailWithRegenerateVerificationCode(user.Email, user.UserVerification.CodeHash,notification.Language);
	}
}
