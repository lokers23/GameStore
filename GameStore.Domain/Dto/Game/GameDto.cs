using GameStore.Domain.Dto.Publisher;

namespace GameStore.Domain.Dto.Game;

public class GameDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public DeveloperDto? Developer { get; set; }
    public PublisherDto? Publisher { get; set; }
    public DateTime? ReleaseOn { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public string? VideoUrl { get; set; }
    public string? AvatarName { get; set; }
}