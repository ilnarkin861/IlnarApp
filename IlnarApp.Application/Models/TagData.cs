using System.ComponentModel.DataAnnotations;


namespace IlnarApp.Application.Models;


public class TagData
{
	[Required(ErrorMessage = "Название тега не должно быть пустым")]
	public required string Title { get; set; } 
}