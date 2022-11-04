#nullable disable

namespace GameStore.Domain.Models
{
    public partial class Key
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int? GameId { get; set; }
        public int? ActivationId { get; set; }
        public bool IsUsed { get; set; }

        public virtual Activation Activation { get; set; }
        public virtual Game Game { get; set; }
    }
}
