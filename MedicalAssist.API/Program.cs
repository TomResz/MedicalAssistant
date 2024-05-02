using MedicalAssist.Infrastructure;
using MedicalAssist.Application;
using MedicalAssist.Infrastructure.Middleware;
using Serilog;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

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
