using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.Exceptions;
public sealed class EmptyMedicineNameException : BadRequestException
{
	public EmptyMedicineNameException() : base("Name of medicine cannot be empty.")
	{

	}
}
