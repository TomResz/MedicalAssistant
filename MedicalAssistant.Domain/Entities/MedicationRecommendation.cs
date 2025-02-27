﻿using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Entities;
public class MedicationRecommendation
{
    public MedicationRecommendationId Id { get; private set; }
    public VisitId? VisitId { get; private set; }
    public UserId UserId { get; set; }
    public Note ExtraNote { get; private set; }
    public Date CreatedAt { get; private set; }
    public Medicine Medicine { get; private set; }
    public Date StartDate { get; private set; }
    public Date EndDate { get; private set; }
    public User User { get;private set; }
    public Visit? Visit { get; private set; }

    private readonly HashSet<MedicationRecommendationNotification> _notifications = [];
    public IEnumerable<MedicationRecommendationNotification> Notifications => _notifications;

	private readonly HashSet<RecommendationUsage> _recommendationUsages = [];

    public IEnumerable<RecommendationUsage> RecommendationUsages => _recommendationUsages;


	private MedicationRecommendation() { }
    private MedicationRecommendation(
		MedicationRecommendationId id,
        UserId userId,
		VisitId? visitId,
		Note extraNote,
		Date createdAt,
		Medicine medicine,
		Date startDate,
		Date endDate)
    {
        Id = id;
        UserId = userId;
        VisitId = visitId;
        ExtraNote = extraNote;
        CreatedAt = createdAt;
        Medicine = medicine;
        StartDate = startDate;
        EndDate = endDate;
    }

    public static MedicationRecommendation Create(
		VisitId? visitId,
        UserId userId,
		Note extraNote,
		Date createdAt,
		Medicine medicine,
		Date startDate,
		Date endDate)
    {
        if (startDate > endDate)
        {
            throw new InvalidEndDateException();
        }

        MedicationRecommendation recommendation = new MedicationRecommendation(
            Guid.NewGuid(),
            userId,
			visitId,
			extraNote,
            createdAt,
            medicine,
            startDate, 
            endDate);

        return recommendation;
    }

	public void Update(Medicine medicine, Date start, Date end, VisitId? visitId, string? extraNote)
	{
        if(end <= start)
        {

        }
        Medicine = medicine;
        StartDate = start;
        EndDate = end;
        VisitId = visitId;
        ExtraNote = extraNote;
	}

	internal void ChangeVisitId(VisitId id)
	{
        VisitId = id;
	}

	public void DeleteVisitId()
	{
        VisitId = null;
	}

    public void AddMedication(MedicationRecommendationNotification notification)
    {
        _notifications.Add(notification);
    }

    public RecommendationUsage AddUsage(string timeOfDay,Date createdAt)
    {
        var usuage = RecommendationUsage.Create(timeOfDay, createdAt,Id);
        _recommendationUsages.Add(usuage);
        return usuage;
	}
}
