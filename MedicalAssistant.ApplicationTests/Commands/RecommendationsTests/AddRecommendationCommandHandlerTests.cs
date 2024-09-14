using FluentAssertions;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Recommendations.Commands.AddRecommendation;
using MedicalAssistant.Application.Tests.ObjectFactories;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.DomainServices;
using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace MedicalAssistant.Application.Tests.Commands.RecommendationsTests;
public class AddRecommendationCommandHandlerTests
{
	private readonly IVisitRepository _visitRepository = Substitute.For<IVisitRepository>();
	private readonly IVisitService _visitService = Substitute.For<IVisitService>();
	private readonly IClock _clock = Substitute.For<IClock>();
	private readonly IUserContext _userContext = Substitute.For<IUserContext>();
	private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

	private readonly AddRecommendationCommandHandler _handler;

	private readonly AddRecommendationCommand _command = new(
		Guid.NewGuid(),
		"Note",
		"Medicine",
		2,
		TimeOfDay.Morning,
		   _date,
			_date.AddMonths(1));

	private static readonly DateTime _date = DateTime.Now;

	public AddRecommendationCommandHandlerTests()
    {
		_clock.GetCurrentUtc().Returns(_date);
		_handler = new(
			_visitRepository,
			  _clock,
			  _userContext,
			  _visitService,
			   _unitOfWork);
    }

	[Fact]
	public async Task Handle_ValidRecommendation_AddsRecommendation()
	{
		var user = UserFactory.CreateUser();
		var visit = VisitFactory.Create(user.Id);
		_userContext.GetUserId.Returns(user.Id);
		_visitRepository.GetByIdWithRecommendationsAsync(visit.Id, default).Returns(visit);
		var command = _command with { VisitId = visit.Id };


		await _handler.Handle(command, default);

		_visitService.Received(1).AddRecommendation(visit, user.Id, Arg.Any<Recommendation>());
		_visitRepository.Received(1).Update(visit);
		await _unitOfWork.Received(1).SaveChangesAsync();
	}

	[Fact]
	public async Task Handle_UnknownVisit_ThrowsUnknownVisitException()
	{
		var user = UserFactory.CreateUser();
		var visitId = Guid.NewGuid();
		_userContext.GetUserId.Returns(user.Id);
		_visitRepository.GetByIdWithRecommendationsAsync(visitId, default)
			.ReturnsNull();
		var command = _command with { VisitId = visitId};


		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<UnknownVisitException>();

		_visitService.DidNotReceive().AddRecommendation(Arg.Any<Visit>(), user.Id, Arg.Any<Recommendation>());
		_visitRepository.DidNotReceive().Update(Arg.Any<Visit>());
		await _unitOfWork.DidNotReceive().SaveChangesAsync();
	}
}
