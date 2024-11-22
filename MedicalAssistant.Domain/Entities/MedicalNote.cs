using System.ComponentModel.DataAnnotations;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Entities;

public class MedicalNote
{
    public MedicalNoteId Id { get; private set; }
    public UserId UserId { get; private set; }
    public Note Note { get; private set; }
    public Date CreatedAt { get; private set; }
    public string[] Tags { get; private set; }

    // EF
    public User User { get; private set; }
    protected MedicalNote()
    {
    }

    private MedicalNote(
        Guid id,
        UserId userId,
        Note note,
        Date createdAt,
        string[] tags)
    {
        Id = id;
        UserId = userId;
        Note = note;
        CreatedAt = createdAt;
        Tags = tags;
    }
    
    
    public static MedicalNote Create(
        UserId userId,
        Note note,
        Date createdAt,
        string[] tags)
    {
        var normalizedTags = NormalizeTags(tags);
        
        return new MedicalNote(
            Guid.NewGuid(),
            userId,
            note,
            createdAt,
            normalizedTags);
    }

    public static string[] NormalizeTags(string[] tags)
    {
        var normalizedTags = tags
            .Where(tag => !string.IsNullOrWhiteSpace(tag))
            .Select(tag => tag.Trim().ToUpperInvariant()) 
            .Distinct() 
            .ToArray();
        
        return normalizedTags;
    }
    public void Update(Note note, string[] tags)
    {
        Note = note;
        Tags = tags;
    }
}