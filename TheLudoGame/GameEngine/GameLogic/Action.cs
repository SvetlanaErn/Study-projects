using System;
using System.Collections.Generic;
using GameEngine.DataAccess;
using GameEngine.Models;
using GameEngine.Presentation;

namespace GameEngine.GameLogic
{
    public static class Action
    {
        public static bool StartGame(Board board)
        {
            bool running = true;
            bool exit = false;

            while (running)
            {
                foreach (var player in board.Players) // Players move tokens in turn
                {
                    bool playerHasWon = PlayerMakesMove(player, board); // Player moves, if method returns true. The player has won.

                    if (playerHasWon)
                    {
                        running = false;
                        break;
                    }
                    Console.WriteLine("\n\n");
                    var selectedOption = Menu.ShowMenu("Choose the option", new List<string> { "Pass a move to the next player", "Save Game", "Exit" });
                    switch (selectedOption)
                    {
                        case 0:
                            break;

                        case 1:
                            board.LastMadeMove = player;
                            GameState.SaveGame(board); // Save board name, players name, the position of all players' tokens (that are not yet at the finish) and which player made the last move.
                            running = false;
                            break;

                        case 2:
                            running = false;
                            exit = true;
                            break;
                    }
                    if (!running) break;
                }
            }
            return exit;
        }

        public static bool PlayerMakesMove(Player player, Board board) // A player choose a token and moves. If the player wins the method returns true.
        {
            Console.Clear();
            Graphics.Draw(board); // Draw 2D board with tokens
            Console.WriteLine($"Player {player.Name}, Team {player.Tokens[0].Color}");
            Utility.PrintInfo(player);
            Console.WriteLine();
            Console.Write("Dice is rolling... ");
            int dice = Dice.RollDice();
            Console.WriteLine(dice);
            Console.WriteLine();
            var tokensList = Utility.GetTokensList(player);
            bool pass = false;
            bool win = false;

            while (true)
            {
                var selectedOption = Menu.ShowMenu("Choose token: ", tokensList); // The player choose the token that he wants to move
                var selectedToken = player.Tokens[selectedOption];
                var result = selectedToken.Move(board, player, dice); // Call the method that move a token. The method returns a string with result information
                Console.WriteLine(result);
                if (result.Contains("You made a move!") || result == "Token at the finish!")
                {
                    Utility.PrintInfo(player);
                    break;
                }

                if (result == "Win!")
                {
                    Console.Clear();
                    Console.WriteLine(player.Name + " won!");
                    Console.ReadKey();
                    Console.Clear();
                    win = true;
                    break;
                }
                // If the player can't move selected token (i.e. route is blocked)
                selectedOption = Menu.ShowMenu("Choose the option", new List<string> { "Pass a move to the next player", "Select another token" });

                switch (selectedOption)
                {
                    case 0:
                        pass = true;
                        break;

                    case 1:
                        Console.WriteLine("Select another token:");
                        break;
                }

                if (pass) break;
            }

            return win;
        }
    }
}