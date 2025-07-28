using System.ComponentModel.DataAnnotations;


namespace IlnarApp.Application.Models;


public class ArchiveRequest
{
	public string? Id { get; set; }
	
	[Required(ErrorMessage = "Название архива не должно быть пустым")]
	public required string Name { get; set; } 
}