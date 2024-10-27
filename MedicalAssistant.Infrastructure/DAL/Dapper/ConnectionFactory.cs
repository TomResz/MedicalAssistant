using MedicalAssistant.Infrastructure.DAL.Options;
using MedicalAssistant.Infrastructure.Docker;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;

namespace MedicalAssistant.Infrastructure.DAL.Dapper;
internal sealed class ConnectionFactory : ISqlConnectionFactory
{
	private readonly IDockerChecker _dockerChecker;
	private readonly DatabaseOptions _databaseOptions;

	public ConnectionFactory(
		IDockerChecker dockerChecker,
		IOptions<DatabaseOptions> options)
	{
		_dockerChecker = dockerChecker;
		_databaseOptions = options.Value;
	}

	public IDbConnection Create() => new NpgsqlConnection(
		_dockerChecker.IsRunningInContainer 
		? _databaseOptions.DockerConnectionString
		: _databaseOptions.ConnectionString);
}
