namespace IlnarApp.Domain.Note;

using Archive;
using Tag;

public class Note : Entity
{
	public string? Title { get; set; }
	public required string Text { get; set; }
	public required NoteType NoteType { get; set; }
	public DateTime Date { get; set; } = DateTime.UtcNow.AddHours(3);
	public Archive? Archive { get; set; }
	public List<Tag> Tags { get; set; } = [];
}