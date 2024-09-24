using MedicalAssistant.UI;
using MedicalAssistant.UI.Shared.APIClient;
using MedicalAssistant.UI.Shared.Options;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using MedicalAssistant.UI.Shared.Services.Auth;
using MedicalAssistant.UI.Shared.Services.HubToken;
using MedicalAssistant.UI.Shared.Services.Language;
using MedicalAssistant.UI.Shared.Services.Notifications;
using MedicalAssistant.UI.Shared.Services.RefreshToken;
using MedicalAssistant.UI.Shared.Services.Time;
using MedicalAssistant.UI.Shared.Services.User;
using MedicalAssistant.UI.Shared.Services.Verification;
using MedicalAssistant.UI.Shared.Services.Visits;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Radzen;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore( conf =>
{
	conf.AddPolicy("HasExternalProvider",
		   x => x.RequireClaim("HasExternalProvider", "True"));
	conf.AddPolicy("HasInternalAuthProvider",
			x => x.RequireClaim("HasExternalProvider", "False"));
});

builder.Services
	.AddGoogleAuthProvider(builder.Configuration)
	.AddFacebookAuthProvider(builder.Configuration)
	.ConfigureAPI(builder.Configuration);

builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<ITokenManager, TokenManager>();
builder.Services.AddScoped<MedicalAssistAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
	sp => sp.GetRequiredService<MedicalAssistAuthenticationStateProvider>());

builder.Services.AddScoped<ILanguageManager,LanguageManager>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IUserAuthService, UserAuthService>();
builder.Services.AddScoped<IVisitService,VisitService>();
builder.Services.AddScoped<IHubTokenService, HubTokenService>();
builder.Services.AddScoped<IUserVerificationService, UserVerificationService>();
builder.Services.AddScoped<IUserDataService, UserDataService>();
builder.Services.AddScoped<IUserPasswordChangeService, UserPasswordChangeService>();
builder.Services.AddScoped<IVisitNotificationService, VisitNotificationService>();
builder.Services.AddScoped<ILocalTimeProvider,LocalTimeProvider>();

builder.Services.AddRadzenComponents();
builder.Services.AddMudServices(config =>
{
	config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

	config.SnackbarConfiguration.PreventDuplicates = false;
	config.SnackbarConfiguration.NewestOnTop = false;
	config.SnackbarConfiguration.ShowCloseIcon = true;
	config.SnackbarConfiguration.VisibleStateDuration = 10000;
	config.SnackbarConfiguration.HideTransitionDuration = 500;
	config.SnackbarConfiguration.ShowTransitionDuration = 500;
	config.SnackbarConfiguration.SnackbarVariant = MudBlazor.Variant.Filled;
});


builder.Services.AddLocalization();

var host = builder.Build();


using (var sp = host.Services.CreateScope())
{
	const string defaultCulture = "pl-PL";
	var localStorage = sp.ServiceProvider.GetRequiredService<LocalStorageService>();

	var stored = await localStorage.GetValueAsync("Culture");
	CultureInfo culture = new(stored ?? defaultCulture);
	if(stored is null)
	{
		await localStorage.SetValueAsync("Culture", culture.Name);
	}
	CultureInfo.DefaultThreadCurrentCulture = culture;
	CultureInfo.DefaultThreadCurrentUICulture = culture;
}

await host.RunAsync();