using MedicalAssist.API.Endpoints;
using MedicalAssist.API.SwaggerDocs;
using MedicalAssist.API.SwaggerDocs.Security;
using MedicalAssist.Application;
using MedicalAssist.Domain;
using MedicalAssist.Infrastructure;
using MedicalAssist.Infrastructure.BackgroundJobs;
using MedicalAssist.Infrastructure.BackgrounJobs;
using MedicalAssist.Infrastructure.DAL;
using MedicalAssist.Infrastructure.Middleware;
using MedicalAssist.Infrastructure.Notifications;
using Serilog;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerDoc();
builder.Services.AddSwaggerAuthMiddleware();
builder.Services.AddSignalR();

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
            .AllowAnyOrigin();
    });
});


var app = builder.Build();

var endpointGroup = app.MapGroup("api");
app.MapEndpoints(endpointGroup);


if (app.Environment.IsDevelopment())
{
    app.UseSwaggerAuthMiddleware();
    app.ApplyMigrations();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard(app.Configuration);
}

app.UseCors("Frontend");
app.UseSerilogRequestLogging();
app.UseOutboxMessageProcessing();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseInfrastructure();

app.MapHub<NotificationHub>("notifications");

app.Run();
        