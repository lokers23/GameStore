using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.ViewModels.Image;

public class ImageViewModel
{
    [Required(ErrorMessage = "Введите название операционной системы")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Количество символов должно быть от 1 до 100")]
    public string? OperatingSystem { get; set; }
    
    
}