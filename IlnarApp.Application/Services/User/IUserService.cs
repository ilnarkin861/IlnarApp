using IlnarApp.Domain.Identity;

namespace IlnarApp.Application.Services.User;


public interface IUserService
{
	Task<bool> SignUp(string email, string password);
	Task<string> SignIn(string email, string password);
	Task<bool> ChangePassword(string oldPassword, string newPassword);
	Task<ApplicationUser> GetUserInfo();
	Task<bool> ChangeEmail(string newEmail);
	void IsAuthenticated();
}