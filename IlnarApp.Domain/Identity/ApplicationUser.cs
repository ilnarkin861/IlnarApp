using Microsoft.AspNetCore.Identity;


namespace IlnarApp.Domain.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
	public required string FirstName { get; set; }
	public required string LastName { get; set; }
}