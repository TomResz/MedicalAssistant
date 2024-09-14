using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Application.Contracts;
public interface IUserContext
{
    bool IsAuthenticated { get; }
    UserId GetUserId { get; }
}
