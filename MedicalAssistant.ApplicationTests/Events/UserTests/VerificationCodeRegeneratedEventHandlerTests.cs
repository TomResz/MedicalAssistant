using FluentAssertions;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.Tests.ObjectFactories;
using MedicalAssistant.Application.User.Events;
using MedicalAssistant.Domain.Events;
using MedicalAssistant.Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace MedicalAssistant.Application.Tests.Events.UserTests;
public class VerificationCodeRegeneratedEventHandlerTests
{
	private readonly IEmailService _emailService = Substitute.For<IEmailService>();
	private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
	private readonly ILogger<VerificationCodeRegeneratedEventHandler> _logger = Substitute.For<ILogger<VerificationCodeRegeneratedEventHandler>>();
    private readonly VerificationCodeRegeneratedEventHandler _eventHandler;
	public VerificationCodeRegeneratedEventHandlerTests()
    {
        _eventHandler = new(
            _emailService,
            _userRepository,
            _logger);
    }

    [Fact]
    public async Task Handle_UserExists_SendsMailWithRegenerateVerificationCode()
    {
        var user = UserFactory.CreateNotVerifiedUser();
        var @event = new VerificationCodeRegeneratedEvent(
            user.Id, Domain.Enums.Languages.English);
        _userRepository.GetByIdWithUserVerificationAsync(user.Id).Returns(user);

        await _eventHandler.Handle(@event, default);

        await _emailService.Received(1).SendMailWithRegenerateVerificationCode(user.Email, Arg.Any<string>(), @event.Language);
	}

	[Fact]
	public async Task Handle_UserNotExists_ThrowsUserNotFoundException()
	{
        var userId = Guid.NewGuid();
		var @event = new VerificationCodeRegeneratedEvent(
			userId, Domain.Enums.Languages.English);
		_userRepository.GetByIdWithUserVerificationAsync(userId).ReturnsNull();

		Func<Task> act = async () => await _eventHandler.Handle(@event, default);


        await act.Should().ThrowAsync<UserNotFoundException>();
		await _emailService.DidNotReceive().SendMailWithRegenerateVerificationCode(Arg.Any<string>(), Arg.Any<string>(), @event.Language);
	}
}
