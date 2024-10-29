using Dapper;
using System.Data;

namespace MedicalAssistant.Infrastructure.DAL.Dapper;
internal sealed class SqlTimeOnlyMapper : SqlMapper.TypeHandler<TimeOnly>
{
	public override TimeOnly Parse(object value) 
		=> TimeOnly.FromTimeSpan((TimeSpan)value);

	public override void SetValue(IDbDataParameter parameter, TimeOnly value) 
		=> parameter.Value = value.ToString();
}

