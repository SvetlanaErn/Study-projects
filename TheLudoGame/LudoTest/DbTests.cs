using GameEngine;
using System.Linq;
using Xunit;

namespace LudoTest
{
    public class DbTests
    {
        [Fact]
        public void Save_Board_To_DB()
        {
            using var db = new TestDatabase();
            using var context = db.CreateContext();

            var board = GameFactory.CreateBoard("myTestBoard");
            board.Id = 2;
            var greenPlayer = GameFactory.NewPlayer("greenPlayer", board, TokenColor.Green);
            greenPlayer.Id = 1;
            board.Players.Add(greenPlayer);
            board.Players[0].Tokens[0].Id = 1;

            context.Boards.Add(board);
            context.SaveChanges();

            Assert.Equal(board, context.Boards.Single(b => b.Id == 2));
            Assert.Equal(greenPlayer, context.Players.First());
            Assert.Equal(board.Players[0].Tokens[0], context.Tokens.First());
        }

        [Fact]
        public void Remove_Board_From_DB()
        {
            using var db = new TestDatabase();
            using var context = db.CreateContext();

            var board = GameFactory.CreateBoard("myTestBoard");
            board.Id = 1;
            var greenPlayer = GameFactory.NewPlayer("greenPlayer", board, TokenColor.Green);
            greenPlayer.Id = 1;
            board.Players.Add(greenPlayer);
            board.Players[0].Tokens[0].Id = 1;
            context.Boards.Add(board);
            context.SaveChanges();

            context.Boards.Remove(board);
            context.SaveChanges();

            Assert.Null(context.Boards.FirstOrDefault());
            Assert.Null(context.Players.FirstOrDefault());
            Assert.Null(context.Tokens.FirstOrDefault());
        }
    }
}