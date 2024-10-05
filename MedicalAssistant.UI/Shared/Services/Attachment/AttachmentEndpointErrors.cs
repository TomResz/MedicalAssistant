using MedicalAssistant.UI.Shared.Resources;

namespace MedicalAssistant.UI.Shared.Services.Attachment;

public static class AttachmentEndpointErrors
{
	public static string MatchErrors(string error)
	{
		return error switch
		{
			"InvalidFileExtension" => Translations.InvalidFileExtension,
			_ => Translations.SomethingWentWrong,
		};
	}
}
