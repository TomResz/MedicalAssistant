using FluentAssertions;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Tests.ObjectFactories;
using MedicalAssistant.Application.Visits.Commands.AddVisit;
using MedicalAssistant.Application.Visits.Commands.DeleteVisit;
using MedicalAssistant.Application.Visits.Commands.EditVisit;
using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace MedicalAssistant.Application.Tests.Commands.VisitTests;
public class DeleteVisitCommandHandlerTests
{
	private readonly IVisitRepository _visitRepository = Substitute.For<IVisitRepository>();
	private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
	private readonly DeleteVisitCommandHandler _handler;

	public DeleteVisitCommandHandlerTests()
	{
		_handler = new(
			_unitOfWork,
			 _visitRepository);
	}


	[Fact]
	public async Task Handle_KnownVisit_DeletesVisit()
	{
		var visit = VisitFactory.Create(Guid.NewGuid());
		var command = new DeleteVisitCommand(visit.Id);
		_visitRepository.GetByIdAsync(visit.Id, default).Returns(visit);

		await _handler.Handle(command, default);

		_visitRepository.Received(1).Remove(visit);
		await _unitOfWork.Received(1).SaveChangesAsync();
	}


	[Fact]
	public async Task Handle_UnknownVisit_ThrowsUnknownVisitException()
	{
		var id = Guid.NewGuid();
		var command = new DeleteVisitCommand(id);
		_visitRepository.GetByIdAsync(id, default).ReturnsNull();

		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<UnknownVisitException>();

		_visitRepository.DidNotReceive().Remove(Arg.Any<Visit>());
		await _unitOfWork.DidNotReceive().SaveChangesAsync();
	}
}
