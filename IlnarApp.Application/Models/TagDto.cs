using System.ComponentModel.DataAnnotations;


namespace IlnarApp.Application.Models;


public class TagDto
{
	[Required(ErrorMessage = "Название тега не должно быть пустым")]
	public required string Name { get; set; } 
}