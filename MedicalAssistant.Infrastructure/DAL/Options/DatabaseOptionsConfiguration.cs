using MedicalAssistant.Infrastructure.Docker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace MedicalAssistant.Infrastructure.DAL.Options;
internal sealed class DatabaseOptionsConfiguration : IConfigureOptions<DatabaseOptions>
{
    private readonly IConfiguration _configuration;
    private readonly IDockerChecker _dockerChecker;

    private const string Section = "postgres";

    public DatabaseOptionsConfiguration(
        IConfiguration configuration, 
        IDockerChecker dockerChecker)
    {
        _configuration = configuration;
        _dockerChecker = dockerChecker;
    }

    public void Configure(DatabaseOptions options)
    {
        if(_dockerChecker.IsRunningInContainer)
        {
            options.ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                 ?? throw new ArgumentNullException();
            return;
        }
        _configuration
            .GetSection(Section)
            .Bind(options);

    }
}
