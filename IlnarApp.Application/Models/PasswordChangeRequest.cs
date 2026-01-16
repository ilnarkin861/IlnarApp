using System.ComponentModel.DataAnnotations;

namespace IlnarApp.Application.Models;


public class PasswordChangeRequest
{
    [Required(ErrorMessage = "Пароль не должен быть пустым")]
    public required string OldPassword { get; set; }
    
    [MinLength(8, ErrorMessage = "Пароль должен состоять как минимум из 8 символов")]
    public required string NewPassword { get; set; }
    
    [Required(ErrorMessage = "Подтвердите пароль")]
    [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
    public required string ConfirmedPassword { get; set; }
}