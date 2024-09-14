
using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions;
public sealed class EmptyCityException : BadRequestException
{
	public EmptyCityException() : base("Empty city field.")
	{
	}
}
