﻿using FluentAssertions;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.Security;
using MedicalAssistant.Application.Tests.ObjectFactories;
using MedicalAssistant.Application.User.Commands.RegenerateVerificationCode;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.Enums;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace MedicalAssistant.Application.Tests.Commands.UserTests;
public class RegenerateVerificationCodeCommandHandlerTests
{
	private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
	private readonly ICodeVerification _codeVerification = Substitute.For<ICodeVerification>();
	private readonly IClock _clock = Substitute.For<IClock>();
	private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
	private readonly IUserLanguageContext _userLanguageContext = Substitute.For<IUserLanguageContext>();

	private static readonly DateTime _date = DateTime.UtcNow.AddHours(2);

	private readonly RegenerateVerificationCodeCommandHandler _handler;
	public RegenerateVerificationCodeCommandHandlerTests()
    {
		_clock.GetCurrentUtc().Returns(_date);
		_userLanguageContext.GetLanguage().Returns(Languages.Polish);
		_handler = new(
			_userRepository,
			_codeVerification,
			   _clock,
				 _unitOfWork,
					_userLanguageContext);
    }

	[Fact]
	public async Task Handle_KnownAndUnverifiedUser_RegeneratesCode()
	{
		var user = UserFactory.CreateNotVerifiedUser();

		var command = new RegenerateVerificationCodeCommand(user.Email);
		_userRepository.GetByEmailWithUserVerificationAsync(user.Email).Returns(user);
		_codeVerification.Generate(_date).Returns("code");

		await _handler.Handle(command, default);

		await _unitOfWork.Received(1).SaveChangesAsync();
	}


	[Fact]
	public async Task Handle_VerifiedUser_ThrowsAccountIsAlreadyVerifiedException()
	{
		var user = UserFactory.CreateUser();
		var command = new RegenerateVerificationCodeCommand(user.Email);
		_userRepository.GetByEmailWithUserVerificationAsync(user.Email).Returns(user);
		_codeVerification.Generate(_date).Returns("code");

		Func<Task> act = async () =>  await _handler.Handle(command, default);


		await act.Should().ThrowAsync<AccountIsAlreadyVerifiedException>();
		await _unitOfWork.DidNotReceive().SaveChangesAsync();
	}


	[Fact]
	public async Task Handle_UnknownUser_ThrowsUserNotFoundException()
	{
		var user = UserFactory.CreateUser();
		var command = new RegenerateVerificationCodeCommand(user.Email);
		_userRepository.GetByEmailWithUserVerificationAsync(user.Email).ReturnsNull();

		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<UserNotFoundException>();
		await _unitOfWork.DidNotReceive().SaveChangesAsync();
	}
}
