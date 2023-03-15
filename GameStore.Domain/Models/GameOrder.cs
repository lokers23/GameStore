#nullable disable

namespace GameStore.Domain.Models
{
    public partial class GameOrder
    {
        public int OrderId { get; set; }
        public int GameId { get; set; }

        public virtual Order Order { get; set; }
        public virtual Game Game { get; set; }
    }
}
