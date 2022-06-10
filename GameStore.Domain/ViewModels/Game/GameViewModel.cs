using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Domain.ViewModels.Game
{
    public class GameViewModel
    {
        public string Name { get; set; }
        public string Discription { get; set; }
        public DateTime Created { get; set; }
        public decimal Price { get; set; }
    }
}
