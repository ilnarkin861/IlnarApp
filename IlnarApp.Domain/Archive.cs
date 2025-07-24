namespace IlnarApp.Domain;


public class Archive : Entity
{
	public required string Name { get; set; }
	public List<Note> Notes { get; set; } = [];
}