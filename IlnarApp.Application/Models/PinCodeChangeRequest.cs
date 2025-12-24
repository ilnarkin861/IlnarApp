using System.ComponentModel.DataAnnotations;

namespace IlnarApp.Application.Models;


public class PinCodeChangeRequest
{
	[Required(ErrorMessage = "PIN-код не должен быть пустым")]
	[MinLength(4, ErrorMessage = "PIN-код должен быть не меньше 4 символов")]
	[MaxLength(4, ErrorMessage = "PIN-код не должен превышать более 4 символов")]
	[RegularExpression(@"^\d+$", ErrorMessage = "PIN-код должен состоять только из цифр")]
	public required string PinCode { get; set; }
}