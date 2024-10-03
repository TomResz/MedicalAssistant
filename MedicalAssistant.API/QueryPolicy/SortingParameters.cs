using MedicalAssistant.API.QueryPolicy.Exceptions;
using MedicalAssistant.Application.Visits;

namespace MedicalAssistant.API.QueryPolicy;

public class SortingParameters
{
	public static OrderDirection FromString(string direction)
	{
		return direction.ToLower() switch
		{
			"asc" => OrderDirection.Ascending,
			"desc" => OrderDirection.Descending,
			_ => throw new InvalidSortingParametersException(),
		};
	}
}
