namespace MedicalAssist.UI.Shared.Requests;

public class ChangePasswordByEmailRequest
{
    public string Code { get; set; }
    public string NewPassword { get; set; }
}
