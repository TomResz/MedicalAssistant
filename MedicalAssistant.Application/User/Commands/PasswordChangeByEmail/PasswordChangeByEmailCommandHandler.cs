﻿using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.Security;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.User.Commands.PasswordChangeByEmail;
internal sealed class PasswordChangeByEmailCommandHandler : ICommandHandler<PasswordChangeByEmailCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailCodeManager _emailCodeManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserLanguageContext _userLanguageContext;
	public PasswordChangeByEmailCommandHandler(
		IUserRepository userRepository,
		IEmailCodeManager emailCodeManager,
		IUnitOfWork unitOfWork,
		IUserLanguageContext userLanguageContext)
	{
		_userRepository = userRepository;
		_emailCodeManager = emailCodeManager;
		_unitOfWork = unitOfWork;
		_userLanguageContext = userLanguageContext;
	}

	public async Task Handle(PasswordChangeByEmailCommand request, CancellationToken cancellationToken)
    {
        var language = _userLanguageContext.GetLanguage();
        var user = await _userRepository.GetByEmailAsync(request.Email,cancellationToken);
        
        if (user is null)
        {
            throw new UserNotFoundException(request.Email);
        }

        if (user.HasExternalLoginProvider)
        {
            throw new UserWithExternalProviderCannotChangePasswordException();
		}

        var code = _emailCodeManager.Encode(user.Email);
        user.SendEmailForPasswordChange(code,language);
        await _unitOfWork.SaveChangesAsync(cancellationToken);   
    }
}
