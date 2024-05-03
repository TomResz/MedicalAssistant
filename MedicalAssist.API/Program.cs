using MedicalAssist.Infrastructure;
using MedicalAssist.Application;
using MedicalAssist.Infrastructure.Middleware;
using Serilog;
using MedicalAssist.Domain;
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseInfrastructure();

app.Run();
