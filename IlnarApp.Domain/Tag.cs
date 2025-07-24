namespace IlnarApp.Domain;


public class Tag : Entity
{
	public required string Name { get; set; }
	public List<Note> Notes { get; set; } = [];
}