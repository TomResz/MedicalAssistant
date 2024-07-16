using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Application.Exceptions;
public sealed class InvalidExternalProviderException : ConflictException
{
	public InvalidExternalProviderException(string message) : base(message)
	{

	}
}
