#nullable disable

namespace GameStore.Domain.Models
{
    public partial class Game
    {
        public Game()
        {
            GameGenres = new HashSet<GameGenre>();
            GameMinSpecifications = new HashSet<GameMinSpecification>();
            Keys = new HashSet<Key>();
            Images = new HashSet<Image>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? DeveloperId { get; set; }
        public int? PublisherId { get; set; }
        public int? ActivationId { get; set; }
        public DateTime ReleaseOn { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string VideoUrl { get; set; }
        public string AvatarName { get; set; }

        public virtual Developer Developer { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual Activation Activation { get; set; }
        public virtual ICollection<GameMinSpecification> GameMinSpecifications { get; set; }
        public virtual ICollection<GameGenre> GameGenres { get; set; }
        public virtual ICollection<Key> Keys { get; set; }
        //public virtual ICollection<GameOrder> GameOrders { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}
