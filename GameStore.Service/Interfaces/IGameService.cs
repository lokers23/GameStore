using GameStore.Domain.Entity;
using GameStore.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Service.Interfaces
{
    public interface IGameService
    {
        Task<IBaseResponse<IEnumerable<Game>>> GetGames();
    }
}
