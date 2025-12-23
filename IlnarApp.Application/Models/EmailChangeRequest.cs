using System.ComponentModel.DataAnnotations;

namespace IlnarApp.Application.Models;


public class EmailChangeRequest
{
	[Required(ErrorMessage = "Электронный адрес не должен быть пустым")]
	[RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){1,4})+)$", ErrorMessage = "Некорректный электронный адрес")]
	public required string Email { get; set; }
}