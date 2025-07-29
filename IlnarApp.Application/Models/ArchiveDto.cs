using System.ComponentModel.DataAnnotations;


namespace IlnarApp.Application.Models;


public class ArchiveDto
{
	[Required(ErrorMessage = "Название архива не должно быть пустым")]
	public required string Title { get; set; } 
}