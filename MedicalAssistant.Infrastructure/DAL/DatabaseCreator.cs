using MedicalAssistant.Infrastructure.DAL.Options;
using MedicalAssistant.Infrastructure.Docker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace MedicalAssistant.Infrastructure.DAL;
internal sealed class DatabaseCreator : IDatabaseCreator
{
	private readonly DatabaseOptions _options;
	private readonly ILogger<DatabaseCreator> _logger;
	private readonly IDockerChecker _dockerChecker;

	public DatabaseCreator(
		IOptions<DatabaseOptions> options,
		ILogger<DatabaseCreator> logger,
		IDockerChecker dockerChecker)
	{
		_options = options.Value;
		_logger = logger;
		_dockerChecker = dockerChecker;
	}

	public async Task CreateDatabaseIfNotExists()
	{
		var currentConnectionString =  _options.ConnectionString;

		var connectionString = TrimHostFromConnectionString(currentConnectionString);
		var dbName = GetDatabaseNameFromConnectionString(currentConnectionString);
		var database = dbName is null ? "MedicalAssistDb" : dbName;

		if (_dockerChecker.IsRunningInContainer)
        {
			const int maxRetries = 15;
			int retryCount = 0;	
			_logger.LogInformation("Waiting for database...");
			bool isConnected = false;

			while(retryCount < maxRetries && !isConnected) 
			{
				await Task.Delay(3500);

				try
				{
					using (var testConnection = new NpgsqlConnection(connectionString))
					{
						await testConnection.OpenAsync();
						isConnected = true;
						_logger.LogInformation("Successfully connected to the PostgreSQL server.");
					}
				}
				catch (Exception)
				{
					retryCount++;
					_logger.LogWarning($"Failed to connect to PostgreSQL server. Attempt {retryCount} of {maxRetries}. Retrying in 3.5 seconds...");
				}
			}

			if(!isConnected)
			{
				_logger.LogError($"Could not connect to the PostgreSQL server after {maxRetries} attempts.");
				return;
			}
		}



		using (var connection = new NpgsqlConnection(connectionString))
		{
			await connection.OpenAsync();

			bool databaseExists = await CheckDatabaseExists(connection, database);

			if (!databaseExists)
			{
				using (var command = new NpgsqlCommand($"CREATE DATABASE \"{database}\";", connection))
				{
					try
					{
						await command.ExecuteNonQueryAsync();
						_logger.LogInformation($"The database '{database}' has been successfully created.");
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, $"An error occurred while creating the database '{database}': {ex.Message}");
					}
				}
			}
			else
			{
				_logger.LogInformation($"The database '{database}' already exists.");
			}
		}

	}
	private static string? GetDatabaseNameFromConnectionString(string connectionString)
	{
		var builder = new NpgsqlConnectionStringBuilder(connectionString);
		return builder.Database;
	}
	private static string TrimHostFromConnectionString(string connectionString)
	{
		int hostIndex = connectionString.IndexOf("Database=");
		if (hostIndex != -1)
		{
			int endIndex = connectionString.IndexOf(";", hostIndex);
			if (endIndex != -1)
			{
				string trimmed = connectionString.Remove(hostIndex, endIndex - hostIndex + 1);
				return trimmed;
			}
		}
		return connectionString;
	}

	private static async Task<bool> CheckDatabaseExists(NpgsqlConnection connection, string databaseName)
	{
		using (var command = new NpgsqlCommand($"SELECT 1 FROM pg_database WHERE datname = '{databaseName}';", connection))
		{
			var result = await command.ExecuteScalarAsync();
			return result is not null;
		}
	}
}
