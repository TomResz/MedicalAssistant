using MedicalAssistant.Application.MedicationRecommendations.Commands.DeleteRecommendation;

namespace MedicalAssistant.Application.Tests.Commands.RecommendationsTests;
public class DeleteRecommendationCommandHandlerTests
{

	private readonly DeleteRecommendationCommandHandler _handler;
	public DeleteRecommendationCommandHandlerTests()
	{

	}

	[Fact]
	public async Task Handle_UnknownVisit_ThrowsUnknownVisitException()
	{

	}

	[Fact]
	public async Task Handle_ValidRecommendationId_DeletesRecommendation()
	{

	}
}
