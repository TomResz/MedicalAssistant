using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.Exceptions;
public sealed class InvalidQuantityException : BadRequestException
{
	public InvalidQuantityException() : base("Quantity cannot be less or equal to 0.")
	{

	}
}
