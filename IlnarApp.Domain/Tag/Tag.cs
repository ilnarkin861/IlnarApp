namespace IlnarApp.Domain.Tag;

using Note;


public class Tag : Entity
{
	public required string Name { get; set; }
	public List<Note> Notes { get; set; } = [];
}