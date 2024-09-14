using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions;
public sealed class EmptyMedicineNameException : BadRequestException
{
	public EmptyMedicineNameException() : base("Name of medicine cannot be empty.")
	{

	}
}
