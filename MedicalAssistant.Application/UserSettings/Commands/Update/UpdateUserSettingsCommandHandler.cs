using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;

namespace MedicalAssistant.Application.UserSettings.Commands.Update;
internal sealed class UpdateUserSettingsCommandHandler
	: IRequestHandler<UpdateUserSettingsCommand>
{
	private readonly IUserRepository _userRepository;
	private readonly IUserContext _userContext;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IUserSettingRepository _userSettingRepository;
	public UpdateUserSettingsCommandHandler(
		IUserRepository userRepository,
		IUserContext userContext,
		IUnitOfWork unitOfWork,
		IUserSettingRepository userSettingRepository)
	{
		_userRepository = userRepository;
		_userContext = userContext;
		_unitOfWork = unitOfWork;
		_userSettingRepository = userSettingRepository;
	}

	public async Task Handle(UpdateUserSettingsCommand request, CancellationToken cancellationToken)
	{
		var userId = _userContext.GetUserId;

		var user = await _userRepository.GetByIdWithSettingsAsync(userId, cancellationToken);

		if (user is null)
		{
			throw new UserNotFoundException();
		}

		user.UserSettings.ChangeSetting(
			request.NotificationsEnabled,
			request.EmailNotificationEnabled,
			request.VisitNotificationEnabled,
			NotificationLanguage.FromString( request.NotificationLanguage)
			);

		_userSettingRepository.Update(user.UserSettings);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
	}
}
