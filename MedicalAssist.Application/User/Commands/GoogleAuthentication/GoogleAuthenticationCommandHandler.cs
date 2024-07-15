﻿using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.User.Commands.ExternalAuthentication;
using MedicalAssist.Application.User.Commands.SignIn;

namespace MedicalAssist.Application.User.Commands.GoogleAuthentication;
internal sealed class GoogleAuthenticationCommandHandler
	: IRequestHandler<GoogleAuthenticationCommand, SignInResponse>
{
	private readonly IMediator _mediator;
	private readonly IGoogleAuthService _googleAuthService;
	public GoogleAuthenticationCommandHandler(IMediator mediator, IGoogleAuthService googleAuthService)
	{
		_mediator = mediator;
		_googleAuthService = googleAuthService;
	}

	public async Task<SignInResponse> Handle(GoogleAuthenticationCommand request, CancellationToken cancellationToken)
	{
		var response = await _googleAuthService.AuthenticateUser(request.Code, cancellationToken);
		return await _mediator.Send(new ExternalAuthenticationCommand(response, "google")); 
	}
}
