using System.ComponentModel.DataAnnotations;

namespace IlnarApp.Application.Models;


public class UserLoginRequest
{
    [Required(ErrorMessage = "Электронный адрес не должен быть пустым")]
    [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){1,4})+)$", ErrorMessage = "Некорректный электронный адрес")]
    public required string Email { get; set; }
    
    [Required(ErrorMessage = "Пароль не должен быть пустым")]
    public required string Password { get; set; }
}