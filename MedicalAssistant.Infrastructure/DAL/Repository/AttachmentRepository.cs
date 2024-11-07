using MedicalAssistant.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedicalAssistant.Infrastructure.DAL.Repository;

internal sealed class AttachmentRepository : IAttachmentRepository
{
	private readonly MedicalAssistantDbContext _context;

	public AttachmentRepository(MedicalAssistantDbContext context)
	{
		_context = context;
	}

	public async Task<int> DeleteAsync(Expression<Func<Domain.Entities.Attachment,bool>> predicate)
	{
		var response = await _context
			.Attachments
			.Where(predicate)
			.ExecuteDeleteAsync();

		return response;
	}
}
