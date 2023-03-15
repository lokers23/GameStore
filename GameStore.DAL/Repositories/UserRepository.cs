using GameStore.DAL.Interfaces;
using GameStore.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly GamestoredbContext _db;
        public UserRepository(GamestoredbContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(User model)
        {
            await _db.Users.AddAsync(model);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(User model)
        {
            _db.Users.Remove(model);
            await _db.SaveChangesAsync();
        }

        public IQueryable<User> GetAll()
        {
            return _db.Users;
        }

        // public async Task<User?> GetByIdAsync(int id)
        // {
        //     var user = await _db.Users.FirstOrDefaultAsync(user => user.Id == id);
        //     return user;
        // }

        public async Task UpdateAsync(User model)
        {
            _db.Users.Update(model);
            await _db.SaveChangesAsync();
        }
    }
}
