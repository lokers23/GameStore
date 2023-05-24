using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.ViewModels.Key;

public class KeyViewModel
{
    [Required(ErrorMessage = "Введите ключ")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Количество символов должно быть от 1 до 100")]
    public string? Value { get; set; }
    
    [Required(ErrorMessage = "Выберите игру")]
    public int? GameId { get; set; }
    
    //[Required(ErrorMessage = "Выберите платформу для активации ключа")]
    //public int? ActivationId { get; set; }
    
    [Required(ErrorMessage = "Выберите использованый ли ключ")]
    public bool? IsUsed { get; set; }
}