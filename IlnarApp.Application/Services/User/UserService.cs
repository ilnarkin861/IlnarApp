using System.Security.Claims;
using IlnarApp.Application.Exceptions;
using IlnarApp.Application.Services.Jwt;
using IlnarApp.Domain.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace IlnarApp.Application.Services.User;


public class UserService(
	UserManager<ApplicationUser> userManager, 
	IUserStore<ApplicationUser> userStore,
	SignInManager<ApplicationUser> signInManager,
	IJwtGenerator jwtGenerator,
	IHttpContextAccessor httpContextAccessor) : IUserService
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
	

	public async Task<string> SignIn(string email, string password)
	{
		
		var user = await userManager.FindByEmailAsync(email);
		
		if (user == null)
		{
			throw new ApiException("Пользователь не найден");
		}
		
		var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
		
		if (!result.Succeeded)
		{
			throw new AuthenticationFailureException("Неверный логин или пароль");
		}
		
		return jwtGenerator.GenerateJwt(user.Id.ToString());
	}

	
	public async Task<bool> ChangePassword(string oldPassword, string newPassword)
	{
		var user = await GetCurrentUser();

		if (user == null)
		{
			throw new EntityNotFoundException("Пользователь не найден");
		}
		
		var passwordResult = await userManager.CheckPasswordAsync(user, oldPassword);

		if (!passwordResult)
		{
			throw new ApiException("Неверный старый пароль");
		}
		
		var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
		
		return result.Succeeded;
	}

	
	public async Task<ApplicationUser> GetUserInfo()
	{
		var user = await GetCurrentUser();
		
		if (user == null)
		{
			throw new EntityNotFoundException("Пользователь не найден");
		}
		
		return user;
	}
	

	public async Task<bool> ChangeEmail(string newEmail)
	{
		var user = await GetCurrentUser();

		if (user == null)
		{
			throw new EntityNotFoundException("Пользователь не найден");
		}
		
		var emailResult = await userManager.SetEmailAsync(user, newEmail);
		var userNameResult = await userManager.SetUserNameAsync(user, newEmail);

		if (!userNameResult.Succeeded || !emailResult.Succeeded)
		{
			throw new ApiException("Ошибка при изменении электронного адреса");
		}

		return true;
	}
	

	public Task<bool> ChangePinCode(string oldPinCode, string newPinCode)
	{
		throw new NotImplementedException();
	}

	public Task<bool> CheckPinCode(string pinCode)
	{
		throw new NotImplementedException();
	}


	private async Task<ApplicationUser?> GetCurrentUser()
	{
		var principal = httpContextAccessor.HttpContext?.User;

		var identifier = principal?.FindFirstValue(ClaimTypes.NameIdentifier);
		
		if (identifier != null)
		{
			return await userManager.FindByIdAsync(identifier);
		}

		return null;
	}

}