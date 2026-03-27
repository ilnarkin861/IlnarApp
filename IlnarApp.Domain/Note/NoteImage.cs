namespace IlnarApp.Domain.Note;


public class NoteImage : Entity
{
    public required string Url { get; set; }
	
    [Newtonsoft.Json.JsonIgnore]
    public List<Note> Notes { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(3);
    public bool Deleted { get; set; }
}