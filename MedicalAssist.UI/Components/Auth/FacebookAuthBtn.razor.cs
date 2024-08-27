using MedicalAssist.UI.Shared.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Web;

namespace MedicalAssist.UI.Components.Auth;

public partial class FacebookAuthBtn
{
	[Inject]
	private NavigationManager _navigation { get; set; }

	[Inject]
	private IOptions<FacebookOptions> _options { get; set; }

	public void AuthWithFacebook()
	{
		string redirectUri = HttpUtility.UrlEncode(_options.Value.CallbackPath);
		string clientId = _options.Value.ClientId;

		string state = GenerateState();

		string authorizationUrl = $"https://www.facebook.com/v20.0/dialog/oauth?client_id={clientId}" +
		$"&redirect_uri={redirectUri}" +
		$"&state={Uri.EscapeDataString(state)}&scope=email";

		_navigation.NavigateTo(authorizationUrl);
	}
	public static string GenerateState()
	{
		byte[] data = new byte[32];
		using (var rng = RandomNumberGenerator.Create())
		{
			rng.GetBytes(data);
		}
		return Convert.ToBase64String(data);
	}
}
