using FluentAssertions;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Recommendations.Commands.DeleteRecommendation;
using MedicalAssist.Application.Tests.ObjectFactories;
using MedicalAssist.Domain.DomainServices;
using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.Exceptions;
using MedicalAssist.Domain.Repositories;
using MedicalAssist.Domain.ValueObjects.IDs;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace MedicalAssist.Application.Tests.Commands.RecommendationsTests;
public class DeleteRecommendationCommandHandlerTests
{
	private readonly IVisitRepository _visitRepository = Substitute.For<IVisitRepository>();
	private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
	private readonly IVisitService _visitService = Substitute.For<IVisitService>();
	private readonly IUserContext _userContext = Substitute.For<IUserContext>();

	private readonly DeleteRecommendationCommandHandler _handler;
	public DeleteRecommendationCommandHandlerTests()
	{
		_userContext.GetUserId.Returns(new UserId(Guid.NewGuid()));

		_handler = new(
			_visitRepository,
			_unitOfWork,
			 _visitService,
			  _userContext);
	}

	[Fact]
	public async Task Handle_UnknownVisit_ThrowsUnknownVisitException()
	{
		var request = new DeleteRecommendationCommand(Guid.NewGuid(), Guid.NewGuid());
		_visitRepository.GetByIdWithRecommendationsAsync(request.VisitId, default).ReturnsNull();

		Func<Task> act = async () => await _handler.Handle(request,default);

		await act.Should().ThrowAsync<UnknownVisitException>();
	}

	[Fact]
	public async Task Handle_ValidRecommendationId_DeletesRecommendation()
	{
		var userId = _userContext.GetUserId;
		var visit = VisitFactory.Create(userId);
		var recommendation = RecommendationFactory.Create(visit.Id);
		visit.AddRecommendation(recommendation);

		_visitRepository.GetByIdWithRecommendationsAsync(visit.Id, default).Returns(visit);

		var command = new DeleteRecommendationCommand(visit.Id,userId);

		await _handler.Handle(command,default);

		_visitService.Received(1).RemoveRecommendation(Arg.Any<Visit>(),Arg.Any<UserId>(),Arg.Any<RecommendationId>());
		_visitRepository.Received(1).Update(visit);
		await _unitOfWork.Received(1).SaveChangesAsync();
	}
}
