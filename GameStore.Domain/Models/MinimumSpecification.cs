#nullable disable

namespace GameStore.Domain.Models
{
    public partial class MinimumSpecification
    {
        public MinimumSpecification()
        {
            GameMinSpecification = new HashSet<GameMinSpecification>();
        }
        
        public int Id { get; set; }
        public string OperatingSystem { get; set; }
        public string Processor { get; set; }
        public string Memory { get; set; }
        public string Storage { get; set; }
        public string Graphics { get; set; }
        public int PlatformId { get; set; }

        
        public virtual ICollection<GameMinSpecification> GameMinSpecification { get; set; }
        public virtual Platform Platform { get; set; }
    }
}
