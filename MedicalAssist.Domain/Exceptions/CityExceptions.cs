
using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.Exceptions;
public sealed class EmptyCityException : BadRequestException
{
	public EmptyCityException() : base("Empty city field.")
	{
	}
}
