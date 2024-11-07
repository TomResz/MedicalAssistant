using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.ValueObjects;

public sealed record FileExtension
{
	public string Value { get; }

	public FileExtension(string value)
	{

		if (!Validate(value))
		{
			throw new InvalidFileExtensionException();
		}
		string extension = Path.GetExtension(value);
		Value = extension.ToLower();
	}
	public static implicit operator FileExtension(string value) => new(value);
	public static implicit operator string(FileExtension value) => value.Value;

	private static bool Validate(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return false;
		}
		string[] validExtensions = [".pdf", ".png", ".jpg"];
		return validExtensions.Any(ext => value.EndsWith(ext));
	}
}

