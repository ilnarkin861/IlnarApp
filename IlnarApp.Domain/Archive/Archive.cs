namespace IlnarApp.Domain.Archive;

using Note;

public class Archive : Entity
{
	public required string Name { get; set; }
	public List<Note> Notes { get; set; } = [];
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(3);
}