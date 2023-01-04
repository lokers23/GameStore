using GameStore.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public AccessRole AccessRole { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
