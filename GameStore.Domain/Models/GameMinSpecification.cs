namespace GameStore.Domain.Models;

public partial class GameMinSpecification
{
    public int GameId { get; set; }
    public int MinimumSpecificationId { get; set; }

    public virtual Game Game { get; set; }
    public virtual MinimumSpecification MinimumSpecification { get; set; }
}