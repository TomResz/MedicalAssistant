using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;
public sealed class InvalidExternalProviderException : ConflictException
{
	public InvalidExternalProviderException(string message) : base(message)
	{

	}
}
