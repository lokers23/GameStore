using GameStore.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Intefaces
{
    public interface IGameAction : IActionDB<Game>
    {
        Task<Game> GetByName(string name);

    }
}
