using MedicalAssist.Domain.ComplexTypes;
using MedicalAssist.Domain.Enums;
using MedicalAssist.Domain.Events;
using MedicalAssist.Domain.Exceptions;
using MedicalAssist.Domain.Primitives;
using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Domain.Entites;
public class User : AggregateRoot<UserId>
{
	public Email Email { get; private set; }
	public Password? Password { get; private set; }
	public FullName FullName { get; private set; }
	public Role Role { get; private set; }
	public Date CreatedAtUtc { get; private set; }
	public ExternalUserLogin? ExternalUserProvider { get; private set; }
	public UserVerification? UserVerification { get; private set; }
	public RefreshTokenHolder RefreshTokenHolder { get; private set; } = null;
    public UserSettings UserSettings { get; private set; }
	public bool IsVerified { get; private set; } = false;
	public bool HasExternalLoginProvider { get; private set; } = false;

    private readonly HashSet<Visit> _visits = new();
	public IEnumerable<Visit> Visits => _visits;

	private readonly HashSet<NotificationHistory> _notificationHistories = new();
	public IEnumerable<NotificationHistory> NotificationHistories => _notificationHistories;

	protected User() { }
	
	private User(UserId id, Email email, Password password, FullName fullName, Role role, Date createdAtUtc,bool hasExternalLoginProvider,bool isVerified)
	{
		Id = id;
		Email = email;
		Password = password;
		FullName = fullName;
		Role = role;
		CreatedAtUtc = createdAtUtc;
		HasExternalLoginProvider = hasExternalLoginProvider;
		IsVerified = isVerified;
	}

	// For external login provider
    private User(UserId id, Email email, FullName fullName, Role role, Date createdAtUtc, bool hasExternalLoginProvider, bool isVerified)
    {
        Id = id;
        Email = email;
        FullName = fullName;
        Role = role;
        CreatedAtUtc = createdAtUtc;
        HasExternalLoginProvider = hasExternalLoginProvider;
        IsVerified = isVerified;
    }



    public static User Create(Email email, Password password, FullName fullName, Role role, Date createdAtUtc,CodeHash codeHash,Languages language)
	{
		User user = new User(
			 Guid.NewGuid(),
			 email,
			 password,
			 fullName,
			 role,
			 createdAtUtc,
			 false,
			 false);

		user.RefreshTokenHolder = RefreshTokenHolder.CreateEmpty();
		user.UserVerification = new UserVerification(user.Id,createdAtUtc.AddHours(2),codeHash);
		
		user.UserSettings = UserSettings.Create(user.Id, language);
		user.AddEvent(new UserCreatedEvent(user.Id,language));
		return user;
	}

    public static User CreateByExternalProvider(Email email,FullName fullName, Role role, Date createdAtUtc,ProvidedKey key,Provider provider,RefreshTokenHolder tokenHolder,
		Languages language)
    {
		Guid id = Guid.NewGuid();	
        User user = new(
             Guid.NewGuid(),
             email,
             fullName,
             role,
             createdAtUtc,
             true,
             true);

		user.RefreshTokenHolder = tokenHolder;
		user.ExternalUserProvider = new ExternalUserLogin(id, key, provider);
		user.UserSettings = UserSettings.Create(user.Id,language);
		user.AddEvent(new UserCreatedByExternalLoginProviderEvent(user.Id,provider,language));
        return user;
    }

    public void RevokeRefreshToken() => RefreshTokenHolder.Revoke();

    public void ChangeRefreshTokenHolder(RefreshTokenHolder refreshToken)
	{
		RefreshTokenHolder = refreshToken;
	}


    public void ChangeFullName(FullName fullName)
	{
		if (FullName == fullName)
		{
			throw new SameFullNamesException();
		}
		FullName = fullName;
	}

	public void ChangePassword(Password password)
	{
		if(HasExternalLoginProvider)
		{
			throw new UserWithExternalProviderCannotChangePasswordException();
		}
		Password = password;
	}
	public void AddVisit(Visit visit)
	{
		var isOverlapping = _visits.Any(v =>
			visit.Date < v.PredictedEndDate && 
			visit.PredictedEndDate > v.Date);

		if (isOverlapping)
		{
			throw new VisitAlreadyExistsForGivenPeriodOfTimeException(visit.Date, visit.PredictedEndDate);
		}
		_visits.Add(visit);
	}

	public void VerifyAccount(DateTime currentTime)
	{
		if(UserVerification?.ExpirationDate.Value < currentTime)
		{
			throw new InactiveVerificationCodeException(); 
		}
		IsVerified = true;
	}

	public void RegenerateVerificationCode(string code, DateTime dateTime, Languages language)
	{
		if (IsVerified)
		{
			throw new AccountIsAlreadyVerifiedException();
		}
		UserVerification?.Regenerate(dateTime.AddHours(2), code);
		AddEvent(new VerificationCodeRegeneratedEvent(Id,language));
	}

	public void SendEmailForPasswordChange(string code,Languages language)
	{
		AddEvent(new SendEmailForPasswordChangeEvent(Id, code,language));
	}
}
