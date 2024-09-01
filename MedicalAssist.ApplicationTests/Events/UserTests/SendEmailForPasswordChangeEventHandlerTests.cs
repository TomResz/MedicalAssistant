using FluentAssertions;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Application.Tests.ObjectFactories;
using MedicalAssist.Application.User.Events;
using MedicalAssist.Domain.Enums;
using MedicalAssist.Domain.Events;
using MedicalAssist.Domain.Repositories;
using MedicalAssist.Domain.ValueObjects;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;

namespace MedicalAssist.Application.Tests.Events.UserTests;
public class SendEmailForPasswordChangeEventHandlerTests
{
	private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
	private readonly IEmailService _emailService = Substitute.For<IEmailService>();

    private readonly SendEmailForPasswordChangeEventHandler _eventHandler;
	public SendEmailForPasswordChangeEventHandlerTests()
    {
        _eventHandler = new(
            _userRepository,
            _emailService);
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

		Func<Task> act = async ()  => await _eventHandler.Handle(@event, default);

        await act.Should().ThrowAsync<UserNotFoundException>();
	}
}
