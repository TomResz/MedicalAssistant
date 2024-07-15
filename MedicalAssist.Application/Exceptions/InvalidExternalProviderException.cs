using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Application.Exceptions;
public sealed class InvalidExternalProviderException : BadRequestException
{
	public InvalidExternalProviderException(string message) : base(message)
	{

	}
}
