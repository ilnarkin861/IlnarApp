namespace IlnarApp.Domain.Note;

using Archive;
using Tag;

public class Note : Entity
{
	public string? Title { get; set; }
	public required string Text { get; set; }
	public required NoteType NoteType { get; set; }
	public DateOnly Date { get; set; }
	public Archive? Archive { get; set; }
	public List<Tag> Tags { get; set; } = [];
}