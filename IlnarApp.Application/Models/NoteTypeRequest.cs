using System.ComponentModel.DataAnnotations;


namespace IlnarApp.Application.Models;


public class NoteTypeRequest
{
	public string? Id { get; set; }
	
	[Required(ErrorMessage = "Название типа записи не должно быть пустым")]
	public required string Name { get; set; }
}