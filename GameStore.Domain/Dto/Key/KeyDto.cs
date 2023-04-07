using GameStore.Domain.Dto.Game;
using GameStore.Domain.Dto.Publisher;

namespace GameStore.Domain.Dto.Key;

public class KeyDto
{
    public int? Id { get; set; }
    public string? Value { get; set; }
    public GameDto? Game { get; set; }
    public ActivationDto? Activation { get; set; }
    public bool IsUsed { get; set; }
}