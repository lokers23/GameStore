using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.ViewModels.Platform
{
    public class PlatformViewModel
    {
        [Required(ErrorMessage = "Введите название")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Количество символов должно быть от 1 до 50")]
        public string? Name { get; set; }
    }
}
