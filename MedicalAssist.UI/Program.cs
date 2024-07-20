using MedicalAssist.UI;
using MedicalAssist.UI.Shared.Services.Abstraction;
using MedicalAssist.UI.Shared.Services.Auth;
using MedicalAssist.UI.Shared.Services.RefreshToken;
using MedicalAssist.UI.Shared.Services.User;
using MedicalAssist.UI.Shared.Services.Visits;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using MudBlazor.Services;
using Radzen;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();



builder.Services.AddHttpClient("api", conf =>
{
	conf.BaseAddress = new Uri("https://localhost:7071/api/");
}).AddHttpMessageHandler<HttpClientRequestHandler>();

builder.Services.AddScoped(
	sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("api"));


builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<MedicalAssistAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
	sp=> sp.GetRequiredService<MedicalAssistAuthenticationStateProvider>());

builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IUserAuthService, UserAuthService>();
builder.Services.AddScoped<VisitService>();
builder.Services.AddTransient<HttpClientRequestHandler>();

builder.Services.AddRadzenComponents();
builder.Services.AddMudServices();


builder.Services.AddLocalization();

var host = builder.Build();

const string defaultCulture = "pl-PL";

var js = host.Services.GetRequiredService<IJSRuntime>();
var result = await js.InvokeAsync<string>("blazorCulture.get");
var culture = CultureInfo.GetCultureInfo(result ?? defaultCulture);

if (result is null)
{
	await js.InvokeVoidAsync("blazorCulture.set", defaultCulture);
}

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();