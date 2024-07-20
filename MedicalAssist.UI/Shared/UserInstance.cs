namespace MedicalAssist.UI.Shared;

public class UserInstance
{
	public Guid Id { get; set; }
	public string Email { get; set; }
	public string Role { get; set; }
	public string FullName { get; set; }

	public bool IsVerified { get; set; }
	public bool HasExternalProvider { get; set; }
}
