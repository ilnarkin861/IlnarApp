using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace IlnarApp.Application.Options;


public class AuthOptions
{
	public required string Issuer { get; set; }
	public required string Audience { get; set; }
	public required string Secret { get; set; }
	public int TokenLifetime { get; set; }

	public SymmetricSecurityKey GetSymmetricSecurityKey()
	{
		return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
	}
	
}