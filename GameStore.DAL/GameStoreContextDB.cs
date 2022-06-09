using GameStore.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Design;

namespace GameStore.DAL
{
    public class GameStoreContextDB : DbContext
    {
        public GameStoreContextDB(DbContextOptions<GameStoreContextDB> options)
            :base(options)
        {
            Database.EnsureCreated();   
        }

        public DbSet<Game> games { get; set; }
    }
}