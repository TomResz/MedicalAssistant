using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Events;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.User.Events;
internal sealed class UserCreatedEventHandler 
	: INotificationHandler<UserCreatedEvent>
{
	private readonly IUserRepository _userRepository;
	private readonly IEmailService _emailService;
	public UserCreatedEventHandler(IUserRepository userRepository, IEmailService emailService)
	{
		_userRepository = userRepository;
		_emailService = emailService;
	}

	public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByIdWithUserVerificationAsync(notification.UserId, cancellationToken);
		
		if (user is null)
		{
			throw new UserNotFoundException();
		}

		await _emailService.SendMailWithVerificationCode(user.Email, user.UserVerification!.CodeHash,notification.Language);
	}
}
