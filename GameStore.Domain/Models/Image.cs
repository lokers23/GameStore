namespace GameStore.Domain.Models;

public partial class Image
{
    public int Id { get; set; }
    public string Path { get; set; }
    public int GameId { get; set; }
    
    public virtual Game Game { get; set; }
}