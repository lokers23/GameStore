#nullable disable

namespace GameStore.Domain.Models
{
    public partial class Order
    {
        public Order()
        {
            //GameOrders = new HashSet<GameOrder>();
            KeyOrders = new HashSet<KeyOrder>();
        }

        public int Id { get; set; }
        public DateTime PayOn { get; set; }
        public decimal Amount { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
        //public virtual ICollection<GameOrder> GameOrders { get; set; }
        public virtual ICollection<KeyOrder> KeyOrders { get; set; }
    }
}
