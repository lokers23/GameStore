using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.ViewModels.Account;

public class ChangePasswordViewModel
{
    // [Required(ErrorMessage = "Введите старый пароль")]
    // [StringLength(50, MinimumLength = 6, ErrorMessage = "Длина пароля должна быть от 6 до 50 символов")]
    // public string? OldPassword { get; set; }
    
    [Required(ErrorMessage = "Введите пароль")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Длина пароля должна быть от 6 до 50 символов")]
    public string? NewPassword { get; set; }

    [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
    public string? ConfirmNewPassword { get; set; }
}