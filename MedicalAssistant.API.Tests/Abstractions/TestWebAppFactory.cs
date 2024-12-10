using DotNet.Testcontainers.Builders;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Infrastructure.DAL;
using MedicalAssistant.Infrastructure.DAL.Options;
using MedicalAssistant.Infrastructure.Email;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Testcontainers.PostgreSql;

namespace MedicalAssistant.API.Tests.Abstractions;

public class TestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
	    .WithDatabase("medicalassistant")
	    .WithUsername("postgres")
	    .WithPassword("postgres")
	    .WithWaitStrategy(Wait.ForUnixContainer()
	    	.UntilPortIsAvailable(5432))
	    .Build();

	private IConfiguration _configuration;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {        
        Environment.SetEnvironmentVariable("APITest", "true");

        builder.UseEnvironment("APITest");
        
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<MedicalAssistantDbContext>));

            services.AddDbContext<MedicalAssistantDbContext>(options =>
                options.UseNpgsql(_dbContainer.GetConnectionString()));

            services.RemoveAll(typeof(IEmailSender));
            services.RemoveAll(typeof(IEmailService));

            services.AddSingleton<IEmailSender, TestEmailSender>();
            services.AddSingleton<IEmailService, TestEmailService>();

			services.RemoveAll(typeof(IOptions<DatabaseOptions>));

			services.Configure<DatabaseOptions>(options =>
			{
				options.ConnectionString = _dbContainer.GetConnectionString();
				options.DockerConnectionString = _dbContainer.GetConnectionString();
			});
		});
    }

    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

	public Task DisposeAsync() => _dbContainer.StopAsync();
}