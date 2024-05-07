using MedicalAssist.Domain.Events;
using MedicalAssist.Domain.Exceptions;
using MedicalAssist.Domain.Primitives;
using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Domain.Entites;
public class User : AggregateRoot<UserId>
{
	public Email Email { get; private set; }
	public Password Password { get; private set; }
	public FullName FullName { get; private set; }
	public Role Role { get; private set; }
	public Date CreatedAtUtc { get; private set; }

	public UserVerification UserVerification { get; private set; }
	public bool IsVerified { get; private set; } = false;

    private readonly HashSet<Visit> _visits = new();
	public IEnumerable<Visit> Visits => _visits;
	protected User() { }
	private User(UserId id, Email email, Password password, FullName fullName, Role role, Date createdAtUtc)
	{
		Id = id;
		Email = email;
		Password = password;
		FullName = fullName;
		Role = role;
		CreatedAtUtc = createdAtUtc;
	}

	public static User Create(Email email, Password password, FullName fullName, Role role, Date createdAtUtc,CodeHash codeHash)
	{
		User user = new User(
			 Guid.NewGuid(),
			 email,
			 password,
			 fullName,
			 role,
			 createdAtUtc);
		user.UserVerification = new UserVerification(Guid.NewGuid(),user.Id,createdAtUtc.AddHours(2),codeHash);
	
		user.AddEvent(new UserCreatedEvent(user.Id));
		return user;
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
		Password = password;
	}
	public void AddVisit(Visit visit, int? hourBound = null)
	{
		int hour = (hourBound == null || hourBound < 0)
			? 1
			: (int)hourBound;

		Date lowerBound = visit.Date.Value.AddHours(-hour);
		Date upperBound = visit.Date.Value.AddHours(hour);

		if (_visits.Any(x => x.Date == visit.Date ||
			(x.Date >= lowerBound && x.Date <= upperBound))
			)
		{
			throw new VisitAlreadyExistsForGivenPeriodOfTimeException(lowerBound, upperBound);
		}
		_visits.Add(visit);
	}

	public void RemoveVisit(VisitId visitId)
	{
		bool wasRemoved = _visits.RemoveWhere(x => x.Id.Value == visitId.Value) > 0;
		if (!wasRemoved)
		{
			throw new UnknownVisitException();
		}
	}

	public void VerifyAccount(DateTime currentTime)
	{
		if(UserVerification.ExpirationDate.Value < currentTime)
		{
			throw new InactiveVerificationCodeException(); 
		}
		IsVerified = true;
	}

	public void RegenerateVerificationCode(string code, DateTime dateTime)
	{
		if (IsVerified)
		{
			throw new AccountIsAlreadyVerifiedException();
		}
		UserVerification.Regenerate(dateTime.AddHours(2), code);
		AddEvent(new VerificationCodeRegeneratedEvent(Id));
	}

	public void SendEmailForPasswordChange(string code)
	{
		AddEvent(new SendEmailForPasswordChangeEvent(Id, code));
	}
}
