using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.ViewModels.Game;

public class GameViewModel
{
    [Required(ErrorMessage = "Укажите название")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Укажите разработчика")]
    public int? DeveloperId { get; set; }
    
    [Required(ErrorMessage = "Укажите издателя")]
    public int? PublisherId { get; set; }
    
    [Required(ErrorMessage = "Укажите дату выпуска")]
    public DateTime? ReleaseOn { get; set; }
    
    [Required(ErrorMessage = "Укажите описание")]
    public string? Description { get; set; }
    
    [Required(ErrorMessage = "Укажите цену")]
    public decimal? Price { get; set; }
    
    [Required(ErrorMessage = "Укажите ссылку на видео игры")]
    public string? VideoUrl { get; set; }
    
    public string? AvatarName { get; set; }

    public bool isChangedAvatar { get; set; } = true;

    [Required(ErrorMessage = "Укажите жанры")]
    [MinLength(1)]
    public List<int> GenreIds { get; set; }

    [Required(ErrorMessage = "Укажите минимальную спецификацию")]
    [MinLength(1)]
    public List<int> MinimumSpecificationIds { get; set; }
}