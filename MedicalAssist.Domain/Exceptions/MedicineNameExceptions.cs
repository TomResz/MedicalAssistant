using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions;
public sealed class EmptyMedicineNameException : BadRequestException
{
	public EmptyMedicineNameException() : base("Name of medicine cannot be empty.")
	{

	}
}
