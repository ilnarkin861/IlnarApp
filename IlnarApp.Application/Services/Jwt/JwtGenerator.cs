using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IlnarApp.Application.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IlnarApp.Application.Services.Jwt;


public class JwtGenerator(IOptions<AuthOptions> authOptions) : IJwtGenerator
{
	public string GenerateJwt(string identifier)
	{
		var authParams = authOptions.Value;
            
		var securityKey = authParams.GetSymmetricSecurityKey();

		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
		
		var claims = new List<Claim>
		{
			new(JwtRegisteredClaimNames.Sub, identifier)
		};
		
		var token = new JwtSecurityToken
		(
			authParams.Issuer,
			authParams.Audience,
			claims,
			expires: DateTime.Now.AddSeconds(authParams.TokenLifetime),
			signingCredentials: credentials
		);
		
		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}