using System.Linq;
using GameEngine.Models;

namespace GameEngine.DataAccess
{
    public static class State // Manage connection between game and database
    {
        public static void AddBoard(Board board)
        {
            using var context = new LudoContext();
            context.Board.Add(board);
            context.SaveChanges();
            board.PlayerIDLastMadeMove = board.LastMadeMove.Id;
            context.SaveChanges();
        }

        public static void RemoveBoard(string boardName)
        {
            using var context = new LudoContext();
            var board = context.Board.FirstOrDefault(b => b.BoardName == boardName);
            if (board != null)
            {
                context.Board.Remove(board);
            }
            context.SaveChanges();
        }
    }
}