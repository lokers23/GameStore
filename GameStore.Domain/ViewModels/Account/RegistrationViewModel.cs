using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.ViewModels.Account
{
    public class RegistrationViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите логин")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Длина логина должна быть от 5 до 50 символов")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Длина пароля должна быть от 6 до 50 символов")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Введите почту")]
        [StringLength(50, ErrorMessage = "Длина почты должна быть до 50 символов")]
        [EmailAddress(ErrorMessage = "Некорректно указана почта")]
        public string? Mail { get; set; }
    }
}
