#nullable disable

namespace GameStore.Domain.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public decimal Balance { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
