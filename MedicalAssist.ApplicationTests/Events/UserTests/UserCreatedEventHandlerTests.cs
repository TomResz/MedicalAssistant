using FluentAssertions;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Application.Tests.ObjectFactories;
using MedicalAssist.Application.User.Events;
using MedicalAssist.Domain.Enums;
using MedicalAssist.Domain.Events;
using MedicalAssist.Domain.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace MedicalAssist.Application.Tests.Events.UserTests;
public class UserCreatedEventHandlerTests
{
	private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
	private readonly IEmailService _emailService = Substitute.For<IEmailService>();

    private readonly UserCreatedEventHandler _eventHandler;
	public UserCreatedEventHandlerTests()
    {
        _eventHandler = new(
            _userRepository,
            _emailService);
    }


    [Fact]
    public async Task Handle_UserExists_SendsMailWithVerificationCode()
    {
        var user = UserFactory.CreateNotVerifiedUser();

		_userRepository.GetByIdWithUserVerificationAsync(user.Id).Returns(user);

        var @event = new UserCreatedEvent(
            user.Id,
            Languages.English);

		await _eventHandler.Handle(@event, default);

        await _emailService.SendMailWithVerificationCode(user.Email,Arg.Any<string>(),@event.Language); 
	}

	[Fact]
	public async Task Handle_UserDoesNotExists_ThrowsUserNotFoundException()
	{
		var user = UserFactory.CreateNotVerifiedUser();

		_userRepository.GetByIdWithUserVerificationAsync(user.Id).ReturnsNull();

		var @event = new UserCreatedEvent(
			user.Id,
			Languages.English);

		Func<Task> act = async () => await _eventHandler.Handle(@event, default);

		await act.Should().ThrowAsync<UserNotFoundException>();
	}

}
