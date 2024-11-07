using FluentAssertions;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Tests.ObjectFactories;
using MedicalAssistant.Application.Visits.Commands.AddVisit;
using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;
using NSubstitute;

namespace MedicalAssistant.Application.Tests.Commands.VisitTests;
public class AddVisitCommandHandlerTests
{
	private readonly IUserContext _userContext = Substitute.For<IUserContext>();
	private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
	private readonly IVisitRepository _visitRepository = Substitute.For<IVisitRepository>();
	private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

	private readonly AddVisitCommandHandler _handler;
	private readonly AddVisitCommand _command = new(
		 "City",
		  "11-111",
		 "Street",
			DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
			  "Doctor",
			"Type",
			"Description",
			DateTime.Now.AddMinutes(30).ToString("yyyy-MM-dd HH:mm"));

	public AddVisitCommandHandlerTests()
	{
		_handler = new(
			_userContext,
			  _userRepository,
			  _visitRepository,
			  _unitOfWork);
	}

	[Fact]
	public async Task Handle_ValidCommand_CreatesNewVisit()
	{
		var user = UserFactory.CreateUser();
		_userContext.GetUserId.Returns(user.Id);
		_userRepository.GetUserWithVisitsAsync(user.Id).Returns(user);

		var response = await _handler.Handle(_command, default);

		_visitRepository.Received(1).Add(Arg.Any<Visit>());
		await _unitOfWork.Received(1).SaveChangesAsync();

		response.Should().NotBeNull();
		response.VisitDescription.Should().Be(_command.VisitDescription);
		response.VisitType.Should().Be(_command.VisitType);
		response.Address.Street.Should().Be(_command.Street);
		response.Address.PostalCode.Should().Be(_command.PostalCode);
		response.Address.City.Should().Be(_command.City);
		response.DoctorName.Should().Be(_command.DoctorName);
	}

	[Fact]
	public async Task Handle_InvalidPredictedDate_ThrowsInvalidPredictedDateException()
	{
		var user = UserFactory.CreateUser();
		_userContext.GetUserId.Returns(user.Id);
		var command = _command with
		{
			Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
			PredictedEndDate = DateTime.Now.AddMinutes(-30).ToString("yyyy-MM-dd HH:mm")
		};

		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<InvalidPredictedDateException>();

		_visitRepository.DidNotReceive().Add(Arg.Any<Visit>());
		await _unitOfWork.DidNotReceive().SaveChangesAsync();
		
	}

}
