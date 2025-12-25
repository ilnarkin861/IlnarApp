using System.ComponentModel.DataAnnotations;

namespace IlnarApp.Application.Models;


public class ArchiveRequest
{
	[Required(ErrorMessage = "Название архива не должно быть пустым")]
	public required string Title { get; set; } 
}