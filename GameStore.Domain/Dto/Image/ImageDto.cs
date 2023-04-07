using GameStore.Domain.Dto.Game;

namespace GameStore.Domain.Dto.Image;

public class ImageDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public GameDto? Game { get; set; }
}