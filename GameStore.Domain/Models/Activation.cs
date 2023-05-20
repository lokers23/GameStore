#nullable disable

namespace GameStore.Domain.Models
{
    public partial class Activation
    {
        public Activation()
        {
            //Keys = new HashSet<Key>();
            Games = new HashSet<Game>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        //public virtual ICollection<Key> Keys { get; set; }
        public virtual ICollection<Game> Games { get; set; }
    }
}
