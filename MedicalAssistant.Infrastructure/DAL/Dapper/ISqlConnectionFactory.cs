using System.Data;

namespace MedicalAssistant.Infrastructure.DAL.Dapper;
public interface ISqlConnectionFactory
{
	IDbConnection Create();
}
