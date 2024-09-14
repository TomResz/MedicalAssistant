using FluentAssertions;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.Tests.ObjectFactories;
using MedicalAssistant.Application.User.Commands.RevokeRefreshToken;
using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects.IDs;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

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
		var command = new RevokeRefreshTokenCommand();
		var user = UserFactory.CreateUser();
		user.ChangeRefreshTokenHolder(
			   new RefreshTokenHolder("token",DateTime.UtcNow.AddDays(1)));
		_userRepository.GetByIdAsync(_userId).Returns(user);

		await _handler.Handle(command, default);

		await _unitOfWork.Received(1).SaveChangesAsync();
		user.RefreshTokenHolder.RefreshToken.Should().BeNull();
		user.RefreshTokenHolder.RefreshTokenExpirationUtc.Should().BeNull();
	}

	[Fact]
	public async Task Handle_InvalidUser_ThrowsUserNotFoundException()
	{
		var command = new RevokeRefreshTokenCommand();
		_userRepository.GetByIdAsync(_userId).ReturnsNull();

		Func<Task> act = async () => await _handler.Handle(command, default);

		await act.Should().ThrowAsync<UserNotFoundException>();
		await _unitOfWork.DidNotReceive().SaveChangesAsync();
	}
}