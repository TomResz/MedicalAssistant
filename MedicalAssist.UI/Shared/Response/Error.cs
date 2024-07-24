namespace MedicalAssist.UI.Shared.Response;

public class Error
{
    public static readonly Error None = new ();
    public static readonly Error InternalServerError = new();
}
