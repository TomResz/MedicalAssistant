using MedicalAssistant.Domain.ComplexTypes;

namespace MedicalAssistant.Application.Contracts;
public interface ITokenRepository
{
	void Add(TokenHolder tokenHolder);
}
