#nullable disable

namespace GameStore.Domain.Models
{
    public partial class Game
    {
        public Game()
        {
            GameGenres = new HashSet<GameGenre>();
            Keys = new HashSet<Key>();
            MinimumSpecifications = new HashSet<MinimumSpecification>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? DeveloperId { get; set; }
        public int? PublisherId { get; set; }
        public DateTime ReleaseOn { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string VideoUrl { get; set; }
        public string AvatarPath { get; set; }

        public virtual Developer Developer { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<GameGenre> GameGenres { get; set; }
        public virtual ICollection<Key> Keys { get; set; }
        public virtual ICollection<MinimumSpecification> MinimumSpecifications { get; set; }
        
        public virtual ICollection<GameOrder> GameOrders { get; set; }
    }
}
