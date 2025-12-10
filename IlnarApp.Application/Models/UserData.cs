using System.ComponentModel.DataAnnotations;

namespace IlnarApp.Application.Models;

public class UserData
{
    [Required(ErrorMessage = "Электронный адрес не должен быть пустым")]
    [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){1,4})+)$", ErrorMessage = "Некорректный электронный адрес")]
    public required string Email { get; set; }
    
    [Required(ErrorMessage = "Пароль не должен быть пустым")]
    [MinLength(8, ErrorMessage = "Пароль должен состоять как минимум из 8 символов")]
    public required string Password { get; set; }
    
    [Required(ErrorMessage = "PIN-код не должен быть пустым")]
    [MinLength(4, ErrorMessage = "PIN-код должен быть не меньше 4 символов") ]
    [MaxLength(4, ErrorMessage = "PIN-код не должен превышать 4 символов")]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "PIN-код должен состоять только из цифр")]
    public required string PinCode { get; set; }
    
}