namespace IlnarApp.Domain.Note;


public class NoteType : Entity
{
	public required string Title { get; set; }
	public bool Deleted { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(3);
}