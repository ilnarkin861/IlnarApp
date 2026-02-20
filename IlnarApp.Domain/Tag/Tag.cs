namespace IlnarApp.Domain.Tag;

using Note;


public class Tag : Entity
{
	public required string Title { get; set; }
	
	[Newtonsoft.Json.JsonIgnore]
	public List<Note> Notes { get; set; } = [];
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(3);
	public bool Deleted { get; set; }
}