using MedicalAssistant.UI;
using MedicalAssistant.UI.Shared.APIClient;
using MedicalAssistant.UI.Shared.Options;
using MedicalAssistant.UI.Shared.Services;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using MedicalAssistant.UI.Shared.Services.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Radzen;
using System.Globalization;
using BlazorPro.BlazorSize;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMediaQueryService();
builder.Services.AddResizeListener();

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
builder.Services.AddScoped<MedicalAssistantAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
	sp => sp.GetRequiredService<MedicalAssistantAuthenticationStateProvider>());

builder.Services.AddServices();

builder.Services.AddRadzenComponents();
builder.Services.AddMudServices(config =>
{
	config.PopoverOptions.ThrowOnDuplicateProvider = false;

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