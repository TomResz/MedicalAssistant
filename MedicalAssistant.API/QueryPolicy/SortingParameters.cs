using MedicalAssistant.API.QueryPolicy.Exceptions;
using MedicalAssistant.Application.Visits;

namespace MedicalAssistant.API.QueryPolicy;

public class SortingParameters
{
	public static OrderDirection FromString(string direction)
	{
		if (direction.ToLower() == "asc")
			return OrderDirection.Ascending;
		else if (direction.ToLower() == "desc")
			return OrderDirection.Descending;
		throw new InvalidSortingParametersException();
	}
}
