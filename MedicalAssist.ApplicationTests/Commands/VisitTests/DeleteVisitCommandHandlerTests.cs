using FluentAssertions;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Tests.ObjectFactories;
using MedicalAssist.Application.Visits.Commands.AddVisit;
using MedicalAssist.Application.Visits.Commands.DeleteVisit;
using MedicalAssist.Application.Visits.Commands.EditVisit;
using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.Exceptions;
using MedicalAssist.Domain.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace MedicalAssist.Application.Tests.Commands.VisitTests;
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
