namespace IlnarApp.Application.Services.Jwt;


public interface IJwtGenerator
{
	string GenerateJwt(string identifier);
}