using FluentAssertions;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Application.Tests.ObjectFactories;
using MedicalAssist.Application.User.Events;
using MedicalAssist.Domain.Events;
using MedicalAssist.Domain.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace MedicalAssist.Application.Tests.Events.UserTests;
public class VerificationCodeRegeneratedEventHandlerTests
{
	private readonly IEmailService _emailService = Substitute.For<IEmailService>();
	private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();

    private readonly VerificationCodeRegeneratedEventHandler _eventHandler;
	public VerificationCodeRegeneratedEventHandlerTests()
    {
        _eventHandler = new(
            _emailService,
            _userRepository);
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
