using MedicalAssist.API.SwaggerDocs;
using MedicalAssist.API.SwaggerDocs.Security;
using MedicalAssist.Application;
using MedicalAssist.Domain;
using MedicalAssist.Infrastructure;
using MedicalAssist.Infrastructure.BackgroundJobs;
using MedicalAssist.Infrastructure.BackgrounJobs;
using MedicalAssist.Infrastructure.DAL;
using MedicalAssist.Infrastructure.Middleware;
using Serilog;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerDoc();
builder.Services.AddSwaggerAuthMiddleware();

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

app.MapControllers();
app.UseInfrastructure();

app.Run();
        