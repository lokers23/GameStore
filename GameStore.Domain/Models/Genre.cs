#nullable disable


using Newtonsoft.Json;

namespace GameStore.Domain.Models
{
    public partial class Genre
    {
        public Genre()
        {
            GameGenres = new HashSet<GameGenre>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<GameGenre> GameGenres { get; set; }
    }
}
