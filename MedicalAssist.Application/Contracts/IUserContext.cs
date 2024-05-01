using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Application.Contracts;
public interface IUserContext
{
    bool IsAuthenticated { get; }
    UserId GetUserId { get; }
}
