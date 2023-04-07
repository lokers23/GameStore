using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Domain.Models
{
    public partial class KeyOrder
    {
        public int OrderId { get; set; }
        public int KeyId { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual Key Key { get; set; }
    }
}
