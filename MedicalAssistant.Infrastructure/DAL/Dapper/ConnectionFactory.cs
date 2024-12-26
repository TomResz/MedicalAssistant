using MedicalAssistant.Infrastructure.DAL.Options;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;

namespace MedicalAssistant.Infrastructure.DAL.Dapper;
internal sealed class ConnectionFactory : ISqlConnectionFactory
{
	private readonly DatabaseOptions _databaseOptions;

	public ConnectionFactory(
		IOptions<DatabaseOptions> options)
	{
		_databaseOptions = options.Value;
	}

	public IDbConnection Create() => new NpgsqlConnection( _databaseOptions.ConnectionString);
}
