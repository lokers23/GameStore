#nullable disable

namespace GameStore.Domain.Models
{
    public partial class MinimumSpecification
    {
        public int Id { get; set; }
        public string Os { get; set; }
        public string Processor { get; set; }
        public string Memory { get; set; }
        public string Storage { get; set; }
        public string Graphics { get; set; }
        public int PlatformId { get; set; }
        public int GameId { get; set; }

        public virtual Game Game { get; set; }
        public virtual Platform Platform { get; set; }
    }
}
