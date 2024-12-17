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

ConfigureServices(builder.Services,builder.HostEnvironment, builder.Configuration);

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

static void ConfigureServices(IServiceCollection services, IWebAssemblyHostEnvironment webHostEnv, IConfiguration configuration)
{
	services.AddMediaQueryService();
	services.AddResizeListener();
	services.AddScoped<BlazorPro.BlazorSize.IMediaQueryService, BlazorPro.BlazorSize.MediaQueryService>(); // Add the services
	services.AddAuthorizationCore( conf =>
	{
		conf.AddPolicy("HasExternalProvider",
			x => x.RequireClaim("HasExternalProvider", "True"));
		conf.AddPolicy("HasInternalAuthProvider",
			x => x.RequireClaim("HasExternalProvider", "False"));
	
		conf.AddPolicy("IsActive",
			x=>x.RequireClaim("IsActive", "True"));
		conf.AddPolicy("IsNotActive",
			x=>x.RequireClaim("IsActive", "False"));
	});

	services
		.AddGoogleAuthProvider(configuration)
		.AddFacebookAuthProvider(configuration)
		.ConfigureAPI(configuration);

	services.AddScoped<LocalStorageService>();
	services.AddScoped<ITokenManager, TokenManager>();
	services.AddScoped<MedicalAssistantAuthenticationStateProvider>();
	services.AddScoped<AuthenticationStateProvider>(
		sp => sp.GetRequiredService<MedicalAssistantAuthenticationStateProvider>());

	services.AddServices();

	services.AddRadzenComponents();
	services.AddMudServices(config =>
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
	
	services.AddLocalization();
}