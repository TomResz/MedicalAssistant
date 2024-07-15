using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.User.Commands.ExternalAuthentication;
using MedicalAssist.Application.User.Commands.SignIn;

namespace MedicalAssist.Application.User.Commands.FacebookAuthentication;
internal sealed class FacebookAuthenticationCommandHandler
	: IRequestHandler<FacebookAuthenticationCommand, SignInResponse>
{
	private readonly IMediator _mediator;
	private readonly IFacebookAuthService _facebookAuthService;
	public FacebookAuthenticationCommandHandler(IMediator mediator, IFacebookAuthService facebookAuthService)
	{
		_mediator = mediator;
		_facebookAuthService = facebookAuthService;
	}

	public async Task<SignInResponse> Handle(FacebookAuthenticationCommand request, CancellationToken cancellationToken)
	{
		var response = await _facebookAuthService.AuthenticateUser(request.Code, cancellationToken);
		return await _mediator.Send(new ExternalAuthenticationCommand(response, "facebook"));
	}
}
