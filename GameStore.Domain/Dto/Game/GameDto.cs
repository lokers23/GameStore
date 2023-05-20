using GameStore.Domain.Dto.Activation;
using GameStore.Domain.Dto.Developer;
using GameStore.Domain.Dto.Genre;
using GameStore.Domain.Dto.MinimumSpecification;
using GameStore.Domain.Dto.Publisher;

namespace GameStore.Domain.Dto.Game;

public class GameDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public DeveloperDto? Developer { get; set; }
    public PublisherDto? Publisher { get; set; }
    public ActivationDto? Activation { get; set; }
    public DateTime? ReleaseOn { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public string? VideoUrl { get; set; }
    public string? AvatarName { get; set; }
    public List<GenreDto>? Genres { get; set; }
    public List<MinSpecDto>? MinimumSpecifications { get; set; }
}