using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Models;
using GameEngine.Presentation;
using Microsoft.EntityFrameworkCore;
using Action = GameEngine.GameLogic.Action; // Code breaks without this for some reason

namespace GameEngine.DataAccess
{
    public class GameState
    {
        public static void LoadSavedGame() // Load a saved game by getting the boardId and all entites connected to that board.
        {
            Console.Clear();
            var boardsList = GetListOfSavedGames(); // Return all saved boards

            if (boardsList.Count == 0)
            {
                Console.WriteLine("There are no saved games");
                Console.ReadKey();
            }
            else
            {
                var selectedOption = Menu.ShowMenu("Select board to load game from", boardsList);
                var selectedGameName = boardsList[selectedOption];

                using var context = new LudoContext();
                var board = context.Board // load selected game from DB
                    .Include(b => b.Players)
                    .ThenInclude(p => p.Tokens)
                    .First(b => b.BoardName == selectedGameName);

                board.LastMadeMove = board.Players.Single(p => p.Id == board.PlayerIDLastMadeMove);
                board.Players = NewOrder(board.Players, board.LastMadeMove);

                board.Squares = GameFactory.CreateSquares();

                foreach (var player in board.Players)
                {
                    TokenColor color = player.Tokens[0].Color;
                    int[] route = GameFactory.GetRoute(color);  // Set the route by finding the color of the loaded token(s).

                    foreach (var token in player.Tokens)
                    {
                        token.Route = route; // Assign route to each token
                        if (token.IsActive)
                        {
                            var SquareId = route[token.Steps];
                            Square square = board.Squares.Single(s => s.Id == SquareId);
                            square.Occupants.Add(token);
                        }
                    }
                }

                Console.Clear();
                Console.WriteLine("Summary, press any key to start game.\n");

                foreach (var p in board.Players) // Summary
                {
                    Console.WriteLine($"{p.Name} - Team {p.Tokens[0].Color}");
                    Console.WriteLine();
                    foreach (var t in p.Tokens)
                    {
                        if (t.IsActive)
                        {
                            Console.WriteLine($"Token {t.Color}, steps: {t.Steps}, square: {t.Route[t.Steps]}");
                        }
                    }
                    int tokensInPlay = p.Tokens.Count(x => x.IsActive == true);
                    int tokensOnBase = p.Tokens.Count(x => x.IsActive == false);
                    int tokensAtFinish = 4 - tokensOnBase - tokensInPlay;
                    Console.WriteLine($"Numb" +
                        $"er of tokens on base: {tokensOnBase}");
                    Console.WriteLine($"Number of tokens at the finish: {tokensAtFinish}");
                    Console.WriteLine("\n");
                }
                Console.ReadKey();
                Action.StartGame(board);
            }
        }

        public static void SaveGame(Board board)
        {
            var boardsList = GetListOfSavedGames();

            while (boardsList.Contains(board.BoardName))
            {
                Console.Clear();
                var selectedOption = Menu.ShowMenu("There is already game with such name.\n", new List<string> { "overwrite", "save as a new game" });
                if (selectedOption == 1)
                {
                    Console.Write("Enter a new name for the game: ");
                    board.BoardName = Console.ReadLine();
                }
                else
                {
                    State.RemoveBoard(board.BoardName);
                    break;
                }
            }

            State.AddBoard(board); // Add board to DB
            Console.WriteLine("Saved");
            Console.ReadKey();
        }

        private static List<Player> NewOrder(List<Player> players, Player lastMadeMove) // Change the order of players in the list of players. Order of players in the list determines the order of the moves. A player who made the last move in the game before saving will move last in the continuation of the game. For example a list with players {0,1,2,3} and player 2 made last move in the game before saving. Order of players in contunuation of the game will be {3,0,1,2}
        {
            var newOrder = new List<Player>();
            int numberOfPlayers = players.Count;
            int indexOfLastMadeMove = players.IndexOf(lastMadeMove);
            int numberAfter = numberOfPlayers - 1 - indexOfLastMadeMove;

            var playersAfterLastMadeMove = players.GetRange(indexOfLastMadeMove + 1, numberAfter);
            newOrder.AddRange(playersAfterLastMadeMove);
            var playersBeforeLastMadeMove = players.GetRange(0, indexOfLastMadeMove + 1);
            newOrder.AddRange(playersBeforeLastMadeMove);
            return newOrder;
        }

        private static List<string> GetListOfSavedGames()
        {
            using var context = new LudoContext();
            var boardsList = context.Board.Select(x => x.BoardName).ToList(); // Return all saved games
            return boardsList;
        }
    }
}