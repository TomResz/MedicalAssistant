namespace MedicalAssistant.Infrastructure.DAL;

public interface IDatabaseCreator
{
	Task CreateDatabaseIfNotExists();
}