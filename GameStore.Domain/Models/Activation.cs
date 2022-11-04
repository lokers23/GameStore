#nullable disable

namespace GameStore.Domain.Models
{
    public partial class Activation
    {
        public Activation()
        {
            Keys = new HashSet<Key>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Key> Keys { get; set; }
    }
}
