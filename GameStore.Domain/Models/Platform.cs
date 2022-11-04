#nullable disable

namespace GameStore.Domain.Models
{
    public partial class Platform
    {
        public Platform()
        {
            MinimumSpecifications = new HashSet<MinimumSpecification>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MinimumSpecification> MinimumSpecifications { get; set; }
    }
}
