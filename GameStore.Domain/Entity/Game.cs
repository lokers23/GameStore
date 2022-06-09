using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Domain.Entity
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }
        public DateTime Created { get; set; }
        public decimal Price { get; set; }

    }
}
