using MedicalAssist.API.QueryPolicy.Exceptions;
using MedicalAssist.Application.Visits;

namespace MedicalAssist.API.QueryPolicy;

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
