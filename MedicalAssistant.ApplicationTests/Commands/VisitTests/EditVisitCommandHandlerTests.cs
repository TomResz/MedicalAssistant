using FluentAssertions;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Tests.ObjectFactories;
using MedicalAssistant.Application.Visits.Commands.EditVisit;
using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace MedicalAssistant.Application.Tests.Commands.VisitTests;
public class EditVisitCommandHandlerTests
{
	private readonly IVisitRepository _visitRepository = Substitute.For<IVisitRepository>();
	private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

	private readonly EditVisitCommand _command = new(
		Guid.NewGuid(),
	 "City",
	  "11-111",
	 "Street",
		DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
		  "Doctor",
		"Type",
		"Description",
		DateTime.Now.AddMinutes(30).ToString("yyyy-MM-dd HH:mm"));

	private readonly EditVisitCommandHandler _handler;

	public EditVisitCommandHandlerTests()
	{
		_handler = new(
			_visitRepository,
			_unitOfWork);
	}

	[Fact]
	public async Task Handle_VisitDoesNotExists_ThrowsUnknownVisitException()
	{
		_visitRepository.GetByIdAsync(_command.Id, default).ReturnsNull();

		Func<Task> act = async () => await _handler.Handle(_command, default);

		await act.Should().ThrowAsync<UnknownVisitException>();
		_visitRepository.DidNotReceive().Update(Arg.Any<Visit>());
		await _unitOfWork.DidNotReceive().SaveChangesAsync();
	}

	[Fact]
	public async Task Handle_ConflictingVisitExists_ThrowsVisitAlreadyExistsForGivenPeriodOfTimeException()
	{
		var visit = VisitFactory.Create(Guid.NewGuid());
		var command = _command with { Id = visit.Id };
		_visitRepository.GetByIdAsync(command.Id, default).Returns(visit);
		_visitRepository.HasConflictingVisits(visit.Id, visit.UserId, Arg.Any<Date>(), Arg.Any<Date>(), default)
			.Returns(true);

		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<VisitAlreadyExistsForGivenPeriodOfTimeException>();

		_visitRepository.DidNotReceive().Update(Arg.Any<Visit>());
		await _unitOfWork.DidNotReceive().SaveChangesAsync();
	}


	[Fact]
	public async Task Handle_NoConflictingVisitExists_ShouldUpdatesVisit()
	{
		var visit = VisitFactory.Create(Guid.NewGuid());
		var command = _command with { Id = visit.Id };
		_visitRepository.GetByIdAsync(command.Id, default).Returns(visit);
		_visitRepository.HasConflictingVisits(visit.Id, visit.UserId, Arg.Any<Date>(), Arg.Any<Date>(), default)
			.Returns(false);

		var result = await _handler.Handle(command, default);

		result.Should().NotBeNull();
		_visitRepository.Received(1).Update(visit);
		await _unitOfWork.Received(1).SaveChangesAsync();
		result.Should().BeOfType<VisitDto>();
	}
}
