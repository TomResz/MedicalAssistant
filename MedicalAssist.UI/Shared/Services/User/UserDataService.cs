using MedicalAssist.UI.Shared.Services.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MedicalAssist.UI.Shared.Services.User;

public class UserDataService : IUserDataService
{
	private readonly LocalStorageService _storageService;

	public UserDataService(LocalStorageService storageService)
	{
		_storageService = storageService;
	}

	public async Task<string?> GetUsername()
	{
		var accessToken = await _storageService.GetValueAsync("access_token");

		if (accessToken is null)
		{
			return null;
		}

		var jwtHandler = new JwtSecurityTokenHandler();
		var token = jwtHandler.ReadJwtToken(accessToken);

		return token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
	}
}