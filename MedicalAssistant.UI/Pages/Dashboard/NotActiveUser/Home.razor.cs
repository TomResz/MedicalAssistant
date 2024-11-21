using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using MedicalAssistant.UI.Shared.Services.Auth;
using MedicalAssistant.UI.Shared.Services.RefreshToken;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MedicalAssistant.UI.Pages.Dashboard.NotActiveUser;

public partial class Home
{
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] IUserAuthService UserService { get; set; }
    [Inject] IRefreshTokenService RefreshTokenService { get; set; }
    [Inject] ITokenManager TokenManager { get; set; }
    
    private async Task Reactivate()
    {
        string? refreshToken = await TokenManager.GetRefreshToken();
        string? accessToken = await TokenManager.GetAccessToken();
        
        Response response = await UserService.Reactivate();
        
        if (response.IsFailure)
        {
            await BackToLoginScreen();
        }

        if (string.IsNullOrEmpty(refreshToken)  || 
            string.IsNullOrEmpty(accessToken))
        {
            await BackToLoginScreen();
        }
        
        var newClaims = await RefreshTokenService.RefreshToken(accessToken!,refreshToken!);

        if (newClaims is null)
        {
            await BackToLoginScreen();
        }

        await (AuthenticationStateProvider as MedicalAssistantAuthenticationStateProvider)!.AuthenticateAsync(newClaims!);
        await InvokeAsync(StateHasChanged);

    }
    
    private async Task BackToLoginScreen()
    {
        await (AuthenticationStateProvider as MedicalAssistantAuthenticationStateProvider)!.LogOutAsync();
        await InvokeAsync(StateHasChanged);
    }
}