namespace IlnarApp.Domain;


public class Note : Entity
{
	public string? Title { get; set; }
	public required string Text { get; set; }
	public DateTime Date { get; set; } = DateTime.Now.AddHours(3);
	public Archive? Archive { get; set; }
	public List<Tag> Tags { get; set; } = [];
}