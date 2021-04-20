using GameEngine;
using GameEngine.Models;
using Microsoft.EntityFrameworkCore;

namespace LudoTest
{
    public class TestContext : DbContext
    {
        public DbSet<Board> Boards { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Token> Tokens { get; set; }

        public TestContext()
        {
        }

        public TestContext(DbContextOptions<TestContext> options) : base(options)
        {
        }
    }
}