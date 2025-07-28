using IlnarApp.Domain.Archive;
using IlnarApp.Domain.Note;
using IlnarApp.Domain.Tag;


namespace IlnarApp.Application.Models;


public class NoteFilterRequest
{
	public required NoteType NoteType { get; set; }
	public int? Year { get; set; }
	public int? Month { get; set; }
	public Archive? Archive { get; set; }
	public List<Tag>? Tags { get; set; }
}