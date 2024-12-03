using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Events;
using MedicalAssistant.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MedicalAssistant.Application.User.Events;

internal sealed class VerificationCodeRegeneratedEventHandler : INotificationHandler<VerificationCodeRegeneratedEvent>
{
	private readonly IEmailService _emailService;
	private readonly IUserRepository _userRepository;
	private readonly ILogger<VerificationCodeRegeneratedEventHandler> _logger;
	public VerificationCodeRegeneratedEventHandler(
		IEmailService emailService,
		IUserRepository userRepository,
		ILogger<VerificationCodeRegeneratedEventHandler> logger)
	{
		_emailService = emailService;
		_userRepository = userRepository;
		_logger = logger;
	}

	public async Task Handle(VerificationCodeRegeneratedEvent notification, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByIdWithUserVerificationAsync(notification.UserId, cancellationToken);

		if(user is null)
		{
			_logger.LogWarning("User with ID {UserId} was not found during verification code regeneration.", notification.UserId);
			throw new UserNotFoundException();
		}

		await _emailService.SendMailWithRegenerateVerificationCode(user.Email, user.UserVerification!.CodeHash,notification.Language);
		
		_logger.LogInformation(
			"Verification code regeneration email sent to {Email} for UserId {UserId} with CodeHash {CodeHash} and Language {Language}.",
			user.Email.Value,
			notification.UserId,
			user.UserVerification.CodeHash.Value,
			notification.Language.ToString()
		);
	}
}
