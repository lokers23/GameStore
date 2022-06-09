using GameStore.DAL.Intefaces;
using GameStore.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Repositories
{
    public class GameAction : IGameAction
    {
        private readonly GameStoreContextDB _db;

        public GameAction(GameStoreContextDB db)
        {
            _db = db;
        }

        public async Task<bool> Create(Game entity)
        {
            await _db.games.AddAsync(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Game entity)
        {
            _db.games.Remove(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<Game> Get(int id)
        {
            return await _db.games.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Game> GetByName(string name)
        {
            return await _db.games.FirstOrDefaultAsync(g => g.Name == name);
        }

        public Task<List<Game>> Select()
        {
            return _db.games.ToListAsync();
        }
    }
}
