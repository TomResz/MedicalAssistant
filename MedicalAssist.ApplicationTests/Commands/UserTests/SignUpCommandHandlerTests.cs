using FluentAssertions;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Application.Security;
using MedicalAssist.Application.User.Commands.SignUp;
using MedicalAssist.Domain.Abstraction;
using MedicalAssist.Domain.Repositories;
using MedicalAssist.Domain.ValueObjects;
using NSubstitute;

namespace MedicalAssist.Application.Tests.Commands.UserTests;
public class SignUpCommandHandlerTests
{
    private readonly IPasswordManager _passwordManager = Substitute.For<IPasswordManager>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IClock _clock = Substitute.For<IClock>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ICodeVerification _codeVerification = Substitute.For<ICodeVerification>();
    private readonly IUserLanguageContext _userLanguageContext = Substitute.For<IUserLanguageContext>();
    private readonly IEventPublisher _eventPublisher = Substitute.For<IEventPublisher>();   
    private readonly string _password = "strong-password";

    private readonly SignUpCommandHandler _handler;
    public SignUpCommandHandlerTests()
    {
        _clock.GetCurrentUtc().Returns(DateTime.UtcNow);
        _codeVerification.Generate(Arg.Any<Date>()).Returns("code");
        _passwordManager.Secure(_password).Returns("securedPassword");
        _userLanguageContext.GetLanguage().Returns(Domain.Enums.Languages.Polish);
        _handler = new(
            _passwordManager,
            _userRepository,
            _clock,
            _unitOfWork,
            _codeVerification,
            _userLanguageContext,
            _eventPublisher);
    }

    [Fact]
    public async Task Handle_ValidCredentials_CreatesNewAccount()
    {
        var command = new SignUpCommand(
            "John Travolta",
            "john@gmail.com",
            _password);

        _userRepository.IsEmailUniqueAsync(command.Email).Returns(true);

        await _handler.Handle(command, default);

        await _userRepository.Received(1).AddAsync(Arg.Any<Domain.Entites.User>(), default);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_NotUniqueEmail_ThrowsEmailInUseException()
    {
        var command = new SignUpCommand(
        "John Travolta",
        "john@gmail.com",
        "strong-password");

        _userRepository.IsEmailUniqueAsync(command.Email).Returns(false);

        Func<Task> act = async () => await _handler.Handle(command, default);

        await act.Should().ThrowAsync<EmailInUseException>();

        await _userRepository.DidNotReceive().AddAsync(Arg.Any<Domain.Entites.User>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync();
    }
}
