using MedicalAssistant.UI.Shared.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System.Web;

namespace MedicalAssistant.UI.Components.Auth;

public partial class GoogleAuthBtn
{
	[Inject]
	private NavigationManager _navigation { get; set; }

	[Inject]
	private IOptions<GoogleOptions> _options { get; set; }

	void LoginWithGoogle()
	{
		string redirectUri = HttpUtility.UrlEncode(_options.Value.CallbackPath);
		string clientId = _options.Value.ClientId;

		string fullUri = $"https://accounts.google.com/o/oauth2/v2/auth?access_type=offline&client_id={clientId}" +
		$"&redirect_uri={redirectUri}" +
		"&response_type=code&scope=email%20profile&prompt=consent";

		_navigation.NavigateTo(fullUri);
	}
}
