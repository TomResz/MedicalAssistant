using FluentAssertions;
using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Enums;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.ValueObjects;

namespace MedicalAssistant.Core.Tests.User;
public class UserTests
{
    private static readonly Domain.Entities.User _user = Domain.Entities.User.Create("test@test.com","12345678","Test test",
        Role.User(),DateTime.UtcNow,"Code hashed", Languages.English);

    private readonly Languages _language = Languages.English;
    [Fact]
    public void Create_ValidCredentials_ReturnsNewInstanceOfUser()
    {
        // arrange
        var email = "test@test.com";
        var password = "12345678";
        var role = "user";
        var fullName = "Test Test";
        var createdAt = DateTime.UtcNow;
        var codeHash = "Example hash Code";
        // act 
        var user = Domain.Entities.User.Create(email, password,fullName, role, createdAt, codeHash,_language);  
        
        // assert
        user.Id.Should().NotBe(Guid.Empty);
        user.Role.Value.Should().Be(role);
        user.Password.Value.Should().Be(password);
    }

    [Theory]
    [InlineData("tomtom.com")]
    [InlineData("tom@tom")]
    [InlineData("tom@tom/com")]
    public void Create_InvalidEmailPattern_ThrowsIncorrectEmailPatternException(string initialEmail)
    {
        // arrange
        var email = initialEmail;
        var password = "12345678";
        var role = "user";
        var fullName = "Test Test";
        var createdAt = DateTime.UtcNow;
		var codeHash = "Example hash Code";

		// act 
		Action act = () =>  Domain.Entities.User.Create(email, password, fullName, role, createdAt, codeHash, _language);

        // assert
        act.Should().Throw<IncorrectEmailPatternException>();
    }

	[Fact]
	public void Create_EmptyEmailPattern_ThrowsIncorrectEmailPatternException()
	{
        // arrange
        var email = "";
        var password = _user.Password;
        var role = _user.Role;
        var fullName = _user.FullName;
        var createdAt = _user.CreatedAtUtc;
		var codeHash = "Example hash Code";



		// act 
		Action act = () => Domain.Entities.User
        .Create(email, password, fullName, 
        role, createdAt, codeHash,_language );

		// assert
		act.Should().Throw<EmptyEmailException>();
	}

	[Theory]
    [InlineData("adminn")]
    [InlineData("useruser")]
    public void Create_InvalidRole_ThrowsInvalidRoleException(string initialRole)
    {
        // arrange
        var email = "tom@tom.com";
        var password = "12345678";
        var role = initialRole;
        var fullName = "Test Test";
        var createdAt = DateTime.UtcNow;
		var codeHash = "Example hash Code";

		// act 
		Action act = () => Domain.Entities.User.Create(email, password, fullName, role, createdAt, codeHash, _language);

        // assert
        act.Should().Throw<InvalidRoleException>();
    }

    [Fact]
    public void ChangeName_NewValidName_ShouldChangeName()
    {
        // arrange
        string name = "New fullName";

        // act
        Action act =() => _user.ChangeFullName(name);
        
        // assert
        act.Should().NotThrow();
        _user.FullName.Value.Should().Be(name);
    }

	[Fact]
	public void ChangeName_SameName_ThrowsSameFullNamesException()
	{
		// arrange
		string name = _user.FullName.Value;

		// act
		Action act = () => _user.ChangeFullName(name);

		// assert
		act.Should().Throw<SameFullNamesException>();
	}

	[Fact]
	public void ChangePassword_NewValidPassword_ShouldChangePassword()
	{
        // arrange
        string password = "123456789010";


        // act
        Action act = () => _user.ChangePassword(password);
        
        // assert
        act.Should().NotThrow();
        _user.Password.Value.Should().Be(password); 
	}

	[Fact]
	public void ChangePassword_InvalidPassword_ThrowsInvalidPasswordException()
	{
		// arrange
		string password = "123";


		// act
		Action act = () => _user.ChangePassword(password);

		// assert
		act.Should().Throw<InvalidPasswordLengthException>();
	}

	[Fact]
	public void ChangePassword_EmptyPassword_ThrowsEmptyPasswordException()
	{
		// arrange
		string password = "";


		// act
		Action act = () => _user.ChangePassword(password);

		// assert
		act.Should().Throw<EmptyPasswordException>();
	}

    [Fact]
    public void AddVisit_CorrectCredential_ShouldAddVisit()
    {
        // arrange
        var user = _user;
        var date = DateTime.Now.AddMinutes(6000);
        var end = date.Add(TimeSpan.FromHours(1));
        var visit = Visit.Create(_user.Id, new Address("Kosciuszki 5", "Warsaw", "22-110"), date,"T","T","T",end);

        //act
        user.AddVisit(visit);

        // assert
        user.Visits.Should().Contain(visit);
    }

	[Fact]
	public void AddVisit_VisitAlreadyExistsInSamePeriodOfTime_VisitAlreadyExistsForGivenPeriodOfTimeException()
	{
		// Arrange
		var user = Domain.Entities.User.Create("test@test.com", "12345678", "Test test",
				Role.User(), DateTime.UtcNow, "Code hashed",_language);

		var date = DateTime.Now.AddMinutes(6000);
		var end = date.Add(TimeSpan.FromHours(1));
		var visit = Visit.Create(user.Id, new Address("Kosciuszki 5", "Warsaw", "22-110"), date, "T", "T", "T", end);
		var visitTwo = Visit.Create(user.Id, new Address("Kosciuszki 5", "Warsaw", "22-110"), date.AddMinutes(-10), "T", "T", "T", end);

		user.AddVisit(visit);

		// Act
		Action act = () => user.AddVisit(visitTwo);

		// Assert
		Assert.Throws<VisitAlreadyExistsForGivenPeriodOfTimeException>(() => act());
		user.Visits.Should().NotContain(visitTwo);
	}
}
