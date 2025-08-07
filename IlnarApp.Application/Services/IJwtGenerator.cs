namespace IlnarApp.Application.Services;


public interface IJwtGenerator
{
	string GenerateJwt(string identifier);
}