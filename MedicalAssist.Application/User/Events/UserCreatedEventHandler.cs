using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Domain.Events;
using MedicalAssist.Domain.Repositories;

namespace MedicalAssist.Application.User.Events;
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
