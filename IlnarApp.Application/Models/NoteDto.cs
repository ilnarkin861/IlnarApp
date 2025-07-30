using System.ComponentModel.DataAnnotations;
using IlnarApp.Domain.Archive;
using IlnarApp.Domain.Note;
using IlnarApp.Domain.Tag;


namespace IlnarApp.Application.Models;


public class NoteDto
{
	public string? Title { get; set; }
	
	[Required(ErrorMessage = "Текст записи не должен быть пустым")]
	public required string Text { get; set; }
	
	[Required(ErrorMessage = "Выбери тип записи")]
	public required NoteType NoteType { get; set; }
	
	[Required(ErrorMessage = "Укажи дату")]
	public required DateOnly Date { get; set; }
	
	public Archive? Archive { get; set; }

	public List<Tag>? Tags { get; set; } = [];
}