using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace IlnarApp.Domain.Identity;


public class ApplicationUser : IdentityUser<Guid>
{
	[MinLength(4, ErrorMessage = "PIN-код не должен быть меньше 4 символов")]
	[MaxLength(4, ErrorMessage = "PIN-код не должен быть больше 4 символов")]
	[RegularExpression(@"^\d{4}$", ErrorMessage = "PIN-код должен состоять только из цифр")]
	public required string? PinCode { get; set; }
}