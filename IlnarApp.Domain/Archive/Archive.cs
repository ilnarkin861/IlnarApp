namespace IlnarApp.Domain.Archive;

using Note;

public class Archive : Entity
{
	public required string Name { get; set; }
	public List<Note> Notes { get; set; } = [];
}