namespace MedicalAssistant.Domain.Exceptions.IDs;

public sealed class InvalidMedicationRecommendationIdException : BadImageFormatException
{
    public InvalidMedicationRecommendationIdException() : base("Medication recommendation Id cannot be empty.")
    {

    }
}