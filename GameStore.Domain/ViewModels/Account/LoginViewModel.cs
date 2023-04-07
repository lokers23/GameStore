using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите логин")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Длина логина должна быть от 5 до 50 символов")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Длина пароля должна быть от 6 до 50 символов")]
        public string? Password { get; set; }
    }
}
