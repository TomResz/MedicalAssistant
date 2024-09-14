namespace MedicalAssist.Infrastructure.DAL;

public interface IDatabaseCreator
{
	Task CreateDatabaseIfNotExists();
}