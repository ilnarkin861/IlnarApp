namespace IlnarApp.Application.Services.User;


public interface IUserService
{
	Task<bool> SignUp(string email, string password, string pinCode);
	Task<string> SignIn(string email, string password);
	Task<bool> ChangePassword(string oldPassword, string newPassword);
	Task<String> GetEmail();
	Task<bool> ChangeEmail(string oldEmail, string newEmail);
	Task<bool> ChangePinCode(string oldPinCode, string newPinCode);
	Task<bool> CheckPinCode(string pinCode);
}