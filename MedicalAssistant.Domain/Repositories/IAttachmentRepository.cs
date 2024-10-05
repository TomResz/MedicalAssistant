using MedicalAssistant.Domain.ValueObjects.IDs;
using System.Linq.Expressions;

namespace MedicalAssistant.Domain.Repositories;

public interface IAttachmentRepository
{
	Task<int> DeleteAsync(Expression<Func<Domain.Entites.Attachment, bool>> predicate);
}
