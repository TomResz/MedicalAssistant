using System.Reflection;
using MedicalAssistant.API.Endpoints;
using MedicalAssistant.API.RequestConverters;
using MedicalAssistant.API.SwaggerDocs;
using MedicalAssistant.API.SwaggerDocs.Security;
using MedicalAssistant.Application;
using MedicalAssistant.Domain;
using MedicalAssistant.Infrastructure;
using MedicalAssistant.Infrastructure.BackgroundJobs;
using MedicalAssistant.Infrastructure.DAL;
using MedicalAssistant.Infrastructure.Middleware;
using MedicalAssistant.Infrastructure.Notifications;
using Serilog;
using Spectre.Console;

var builder = WebApplication.CreateBuilder(args);

var isTestEnvironment = Environment.GetEnvironmentVariable("APITest") != null;

builder
	.Configuration
	.AddJsonFile(isTestEnvironment ? "appsettings.Tests.json" : "appsettings.Secrets.json", optional: false, reloadOnChange: true);

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerDoc();
builder.Services.AddSwaggerAuthMiddleware(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddAntiforgery();

builder.Services.ConfigureHttpJsonOptions(opt =>
{
	opt.SerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
});

builder.Services
	.AddApplication()
	.AddInfrastructure(builder.Configuration)
	.AddDomain();

builder.Host.UseSerilog((context, configuration) =>
		configuration
			.ReadFrom
				.Configuration(context.Configuration)
	);

builder.Services.AddCors(options =>
{
	options.AddPolicy("Frontend", builder =>
	{
		builder
			.AllowAnyMethod()
			.AllowAnyHeader()
			.AllowAnyOrigin()
			.WithExposedHeaders("*");
	});
});

AnsiConsole.Write(new FigletText("MedicalAssistant API")
	.LeftJustified()
	.Color(Color.Blue));

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
	var creator = scope.ServiceProvider.GetRequiredService<IDatabaseCreator>();
	await creator.CreateDatabaseIfNotExists();
	app.ApplyMigrations();
}

var endpointGroup = app.MapGroup("api");
app.MapEndpoints(endpointGroup);


if (app.Environment.IsDevelopment())
{
	app.UseSwaggerAuthMiddleware();
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseHangfireDashboard(app.Configuration);
}

app.UseCors("Frontend");
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.UseInfrastructure();
app.UseRecurringBackgroundJobs();
app.MapHub<NotificationHub>("notifications");

app.Run();

public partial class Program;
