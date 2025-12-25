using IlnarApp.Api.Actions;
using IlnarApp.Application.Models;
using IlnarApp.Application.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IlnarApp.Api.Controllers;


public class UserController(IUserService userService) : BaseController
{
    [HttpPost]
    [Route("register")]
    [ValidationAction]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest userRegisterRequest)
    {
        await userService.SignUp(userRegisterRequest.Email, userRegisterRequest.Password, userRegisterRequest.PinCode);
        
        return Ok(new ResponseData{Success = true, Messages = ["Пользователь успешно добавлен"] });
    }


    [HttpPost]
    [Route("login")]
    [ValidationAction]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest userLoginRequest)
    {
        var token = await userService.SignIn(userLoginRequest.Email, userLoginRequest.Password);
        
        return Ok(new UserTokenData{Token = token});
    }


    [HttpPost]
    [Route("password-reset")]
    [ValidationAction]
    public async Task<IActionResult> PasswordReset([FromBody] PasswordChangeRequest request)
    {
        await userService.ChangePassword(request.OldPassword, request.NewPassword);
        
        return Ok(new ResponseData{Success = true, Messages = ["Пароль успешно изменен"] });
    }
    
    
    [HttpPost]
    [Route("email-change")]
    [ValidationAction]
    public async Task<IActionResult> EmailChange([FromBody] EmailChangeRequest request)
    {
        await userService.ChangeEmail(request.Email);
        
        return Ok(new ResponseData{Success = true, Messages = ["Электронный адрес успешно изменен"] });
    }
    
    
    [HttpGet]
    [Route("info")]
    public async Task<IActionResult> UserInfo()
    {
        var user = await userService.GetUserInfo();
        
        return Ok(user);
    }
    
    
    [HttpPost]
    [Route("pincode-change")]
    [ValidationAction]
    public async Task<IActionResult> PinCodeChange([FromBody] PinCodeChangeRequest request)
    {
        await userService.ChangePinCode(request.PinCode);
        
        return Ok(new ResponseData{Success = true, Messages = ["PIN-код успешно изменен"] });
    }
    
    
    [HttpPost]
    [Route("pincode-check")]
    public async Task<IActionResult> PinCodeCheck([FromBody] string pinCode)
    {
        await userService.CheckPinCode(pinCode);
        
        return Ok(new ResponseData{Success = true, Messages = ["PIN-код верный"] });
    }
}