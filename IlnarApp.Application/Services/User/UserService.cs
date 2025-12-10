using IlnarApp.Application.Exceptions;
using IlnarApp.Application.Services.Jwt;
using IlnarApp.Domain.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IlnarApp.Application.Services.User;


public class UserService(
	UserManager<ApplicationUser> userManager, 
	IUserStore<ApplicationUser> userStore,
	SignInManager<ApplicationUser> signInManager,
	IJwtGenerator jwtGenerator,
	IHttpContextAccessor contextAccessor) : IUserService
{
	public async Task<bool> SignUp(string email, string password, string pinCode)
	{
		
		var userExists = await userManager.FindByEmailAsync(email);

		if (userExists != null)
		{
			throw new ApiException("Пользователь уже существует");
		}

		var user = new ApplicationUser{ PinCode = pinCode};
		
		var emailStore = (IUserEmailStore<ApplicationUser>)userStore;
		
		await userStore.SetUserNameAsync(user, email, CancellationToken.None);
		
		await emailStore.SetEmailAsync(user, email, CancellationToken.None);
		
		var result = await userManager.CreateAsync(user, password);

		if (!result.Succeeded)
		{
			throw new ApiException("Ошибка при сохранения пользователя");
		}
		
		return result.Succeeded;
	}
	

	public Task<string> SignIn(string email, string password)
	{
		throw new NotImplementedException();
	}

	public Task<bool> ChangePassword(string oldPassword, string newPassword)
	{
		throw new NotImplementedException();
	}

	public Task<string> GetEmail()
	{
		throw new NotImplementedException();
	}

	public Task<bool> ChangeEmail(string oldEmail, string newEmail)
	{
		throw new NotImplementedException();
	}

	public Task<bool> ChangePinCode(string oldPinCode, string newPinCode)
	{
		throw new NotImplementedException();
	}

	public Task<bool> CheckPinCode(string pinCode)
	{
		throw new NotImplementedException();
	}
}