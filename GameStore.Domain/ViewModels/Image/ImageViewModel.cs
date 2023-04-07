using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.ViewModels.Image;

public class ImageViewModel
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public int? GameId { get; set; }
}