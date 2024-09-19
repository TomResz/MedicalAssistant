using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MedicalAssistant.UI.Shared.Services.Auth;

public class MedicalAssistAuthenticationStateProvider : AuthenticationStateProvider
{
	private readonly ITokenManager _tokenManager;

	public MedicalAssistAuthenticationStateProvider(ITokenManager tokenManager)
	{
		_tokenManager = tokenManager;
	}

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var token = await _tokenManager.GetAccessToken();
		var claims = new ClaimsIdentity();

		if (token is not null)
		{
			claims = GetClaimsFromJwtToken(token);
		}
		var principal = new ClaimsPrincipal(claims);

		return new AuthenticationState(principal);
	}

	public static ClaimsIdentity GetClaimsFromJwtToken(string jwtToken)
	{
		var handler = new JwtSecurityTokenHandler();
		var token = handler.ReadJwtToken(jwtToken);

		var id = token.Claims.First(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
		var fullName = token.Claims.First(x => x.Type == ClaimTypes.Name)!.Value;
		var email = token.Claims.First(x => x.Type == ClaimTypes.Email)!.Value;
		var role = token.Claims.First(x => x.Type == ClaimTypes.Role)!.Value;
		var isVerified = token.Claims.First(x => x.Type == "IsVerified")!.Value;
		var hasExternalProvider = token.Claims.First(x => x.Type == "HasExternalProvider")!.Value;

		return new ClaimsIdentity(new Claim[]
			{
				new(ClaimTypes.Email, email),
				new(ClaimTypes.NameIdentifier, id),
				new(ClaimTypes.Name, fullName),
				new (ClaimTypes.Role, role),
				new ("IsVerified",isVerified),
				new("HasExternalProvider",hasExternalProvider),
			}, "Api");
	}
	public async Task AuthenticateAsync(SignInResponse response)
	{
		await _tokenManager.SetAccessToken(response.AccessToken);
		await _tokenManager.SetRefreshToken(response.RefreshToken);

		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}

	public async Task LogOutAsync()
	{
		await _tokenManager.DeleteAllTokens();
		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}
}
