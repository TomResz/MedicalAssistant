using MedicalAssist.Infrastructure;
using MedicalAssist.Application;
using MedicalAssist.Infrastructure.Middleware;
using Serilog;
using MedicalAssist.Domain;
using MedicalAssist.Infrastructure.DAL;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    app.ApplyMigrations();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Frontend");
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseInfrastructure();

app.Run();
