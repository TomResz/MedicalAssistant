using MedicalAssistant.Domain.ValueObjects.IDs;
using System.Linq.Expressions;

namespace MedicalAssistant.Domain.Repositories;

public interface IAttachmentRepository
{
	Task<int> DeleteAsync(Expression<Func<Domain.Entities.Attachment, bool>> predicate);
}
