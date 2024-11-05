using MedicalAssistant.API.Endpoints;
using MedicalAssistant.API.RequestConverters;
using MedicalAssistant.API.SwaggerDocs;
using MedicalAssistant.API.SwaggerDocs.Security;
using MedicalAssistant.Application;
using MedicalAssistant.Domain;
using MedicalAssistant.Infrastructure;
using MedicalAssistant.Infrastructure.BackgrounJobs;
using MedicalAssistant.Infrastructure.DAL;
using MedicalAssistant.Infrastructure.Middleware;
using MedicalAssistant.Infrastructure.Notifications;
using Microsoft.AspNetCore.Localization;
using Serilog;
using System.Globalization;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);





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

builder.Services.AddCors(options=>
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


var app = builder.Build();


using(var scope = app.Services.CreateScope())
{
	var dbCreater = scope.ServiceProvider.GetRequiredService<IDatabaseCreator>();
    await dbCreater.CreateDatabaseIfNotExists();
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
        