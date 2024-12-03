using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Events;
using MedicalAssistant.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MedicalAssistant.Application.User.Events;
internal sealed class UserCreatedEventHandler 
	: INotificationHandler<UserCreatedEvent>
{
	private readonly IUserRepository _userRepository;
	private readonly IEmailService _emailService;
	private readonly ILogger<UserCreatedEventHandler> _logger;
	public UserCreatedEventHandler(
		IUserRepository userRepository,
		IEmailService emailService,
		ILogger<UserCreatedEventHandler> logger)
	{
		_userRepository = userRepository;
		_emailService = emailService;
		_logger = logger;
	}

	public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByIdWithUserVerificationAsync(notification.UserId, cancellationToken);
		
		if (user is null)
		{
			_logger.LogWarning("User with ID {UserId} was not found during account creation event handling.", notification.UserId);
			throw new UserNotFoundException();
		}

		await _emailService.SendMailWithVerificationCode(user.Email, user.UserVerification!.CodeHash,notification.Language);
		_logger.LogInformation(
			"Verification email sent to {Email} for UserId {UserId} with CodeHash {CodeHash} and Language {Language}.",
			user.Email.Value,
			notification.UserId,
			user.UserVerification.CodeHash.Value,
			notification.Language.ToString()
		);
	}
}
