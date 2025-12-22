using IlnarApp.Api.Actions;
using IlnarApp.Application.Models;
using IlnarApp.Application.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace IlnarApp.Api.Controllers;



public class UserController(IUserService userService) : BaseController
{
    [HttpPost]
    [Route("register")]
    [ValidationAction]
    public async Task<IActionResult> UserRegister([FromBody] UserData userData)
    {
        await userService.SignUp(userData.Email, userData.Password, userData.PinCode);
        
        return Ok(new ResponseData{Success = true, Messages = ["Пользователь успешно добавлен"] });
    }


    [HttpPost]
    [Route("login")]
    [ValidationAction]
    public async Task<IActionResult> UserLogin([FromBody] UserLoginData userLoginData)
    {
        var token = await userService.SignIn(userLoginData.Email, userLoginData.Password);
        
        return Ok(new UserTokenData{Token = token});
    }
    
}