using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Entities;
public class Attachment
{
	public AttachmentId Id { get; set; }
	public VisitId VisitId { get; set; }
	public FileExtension Extension { get; set; }
	public FileName Name { get; set; }
	public FileContent Content { get; set; }


	private Attachment(
		AttachmentId id,
		VisitId visitId,
		FileExtension extension,
		FileName name,
		FileContent fileContent)
	{
		Id = id;
		VisitId = visitId;
		Name = name;
		Extension = extension;
		Content = fileContent;
	}
	public static Attachment Create(
		VisitId visitId,
		FileName name,
		FileContent fileContent)
	{
		return new Attachment(
			Guid.NewGuid(),
			visitId,
			name.Value,
			name,
			fileContent);
	}
	private Attachment() { }	


}
