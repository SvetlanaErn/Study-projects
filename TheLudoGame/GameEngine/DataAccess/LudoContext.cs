using GameEngine.Models;
using Microsoft.EntityFrameworkCore;

namespace GameEngine.DataAccess
{
    public class LudoContext : DbContext
    {
        public DbSet<Board> Board { get; set; }
        public DbSet<Player> Player { get; set; }
        public DbSet<Token> Token { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=LudoGame;IntegratedSecurity=True;");
        }
    }
}