using FluentAssertions;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.Tests.LoggerWrapper;
using MedicalAssistant.Application.Tests.ObjectFactories;
using MedicalAssistant.Application.User.Events;
using MedicalAssistant.Domain.Enums;
using MedicalAssistant.Domain.Events;
using MedicalAssistant.Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace MedicalAssistant.Application.Tests.Events.UserTests;
public class SendEmailForPasswordChangeEventHandlerTests
{
	private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
	private readonly IEmailService _emailService = Substitute.For<IEmailService>();
	private readonly ILogger<SendEmailForPasswordChangeEventHandler> _logger;
	private readonly SendEmailForPasswordChangeEventHandler _eventHandler;
	public SendEmailForPasswordChangeEventHandlerTests()
	{
		_logger = Substitute.For<ILogger<SendEmailForPasswordChangeEventHandler>>();

		_eventHandler = new(
			_userRepository,
			_emailService,
			_logger);
	}

	[Fact]
	public async Task Handle_UserExists_ShouldSendEmail()
	{
		var user = UserFactory.CreateUser();
		_userRepository.GetByIdAsync(user.Id).Returns(user);

		var @event = new SendEmailForPasswordChangeEvent(
			user.Id,
			"code",
			Languages.Polish);

		await _eventHandler.Handle(@event, default);

		await _emailService.Received(1).SendMailWithChangePasswordCode(user.Email, @event.Code, @event.Language);
	}


	[Fact]
	public async Task Handle_UserNotExists_ThrowsUserNotFoundException()
	{
		var user = UserFactory.CreateUser();
		_userRepository.GetByIdAsync(user.Id).ReturnsNull();

		var @event = new SendEmailForPasswordChangeEvent(
			user.Id,
			"code",
			Languages.Polish);

		Func<Task> act = async () => await _eventHandler.Handle(@event, default);

		await act.Should().ThrowAsync<UserNotFoundException>();
	}
}
