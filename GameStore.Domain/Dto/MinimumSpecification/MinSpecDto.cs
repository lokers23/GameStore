using GameStore.Domain.Dto.Platform;
using GameStore.Domain.Models;

namespace GameStore.Domain.Dto.MinimumSpecification;

public class MinSpecDto
{
    public int Id { get; set; }
    public string OperatingSystem { get; set; }
    public string Processor { get; set; }
    public string Memory { get; set; }
    public string Storage { get; set; }
    public string Graphics { get; set; }
    public PlatformDto Platform { get; set; }
}