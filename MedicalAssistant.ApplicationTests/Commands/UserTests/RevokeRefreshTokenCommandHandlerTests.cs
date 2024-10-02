using FluentAssertions;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.Tests.ObjectFactories;
using MedicalAssistant.Application.User.Commands.RevokeRefreshToken;
using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects.IDs;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace MedicalAssistant.Application.Tests.Commands.UserTests;
public class RevokeRefreshTokenCommandHandlerTests
{
	private readonly IUserContext _context = Substitute.For<IUserContext>();
	private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
	private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

	private static readonly Guid _userId = Guid.NewGuid();
	private readonly RevokeRefreshTokenCommandHandler _handler;
	public RevokeRefreshTokenCommandHandlerTests()
	{
		_context.GetUserId.Returns(new UserId(_userId));
		_handler = new(
			_context,
			 _userRepository,
			 _unitOfWork);
	}

	[Fact]
	public async Task Handle_ValidUser_RevokesToken()
	{
		var user = UserFactory.CreateUser();
		user.AddRefreshToken(TokenHolder.Create(
			"refresh-token", DateTime.Now, user.Id));
		_context.GetUserId.Returns(user.Id);
		_userRepository.GetWithRefreshTokens(user.Id, default).Returns(user);
		var command = new RevokeRefreshTokenCommand("refresh-token");


		await _handler.Handle(command, default);

		await _unitOfWork.Received(1).SaveChangesAsync(default);
	}

	[Fact]
	public async Task Handle_InvalidUser_ThrowsUserNotFoundException()
	{
		var user = UserFactory.CreateUser();
		user.AddRefreshToken(TokenHolder.Create(
			"invalid-refresh-token", DateTime.Now, user.Id));
		var command = new RevokeRefreshTokenCommand("refresh-token");


		Func<Task> act = async () => 
			await _handler.Handle(command, default);

		act().ThrowsAsync<UserNotFoundException>();

		await _unitOfWork.DidNotReceive().SaveChangesAsync(default);

	}
}