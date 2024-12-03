using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Infrastructure.DAL;
using MedicalAssistant.Infrastructure.Email;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace MedicalAssistant.API.Tests.Abstractions;

public class TestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("medicalassistantdb")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<MedicalAssistantDbContext>));

            services.AddDbContext<MedicalAssistantDbContext>(options =>
                options.UseNpgsql(_dbContainer.GetConnectionString()));

            services.RemoveAll(typeof(IEmailSender));
            services.RemoveAll(typeof(IEmailService));

            services.AddSingleton<IEmailSender, TestEmailSender>();
            services.AddSingleton<IEmailService, TestEmailService>();
        });
    }
    
    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}