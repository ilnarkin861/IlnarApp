using System.ComponentModel.DataAnnotations;


namespace IlnarApp.Application.Models;


public class NoteTypeData
{
	[Required(ErrorMessage = "Название типа записи не должно быть пустым")]
	public required string Title { get; set; }
}