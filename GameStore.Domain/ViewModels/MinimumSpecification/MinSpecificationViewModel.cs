using System.ComponentModel.DataAnnotations;
namespace GameStore.Domain.ViewModels.MinimumSpecification;

public class MinSpecificationViewModel
{
    [Required(ErrorMessage = "Введите название операционной системы")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Количество символов должно быть от 1 до 100")]
    public string? OperatingSystem { get; set; }
    
    [Required(ErrorMessage = "Введите название процессора")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Количество символов должно быть от 1 до 100")]
    public string? Processor { get; set; }
    
    [Required(ErrorMessage = "Введите необходимое количество оперативной памяти")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Количество символов должно быть от 1 до 100")]
    public string? Memory { get; set; }
    
    [Required(ErrorMessage = "Введите необходимое место на диске")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Количество символов должно быть от 1 до 100")]
    public string? Storage { get; set; }
    
    [Required(ErrorMessage = "Введите название видеокарты")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Количество символов должно быть от 1 до 100")]
    public string? Graphics { get; set; }
    
    [Required(ErrorMessage = "Выберите платформу")]
    public int? PlatformId { get; set; }
}