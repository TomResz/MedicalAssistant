
using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions;
public sealed class EmptyCityException : BadRequestException
{
	public EmptyCityException() : base("Empty city field.")
	{
	}
}
