using System;
using System.Collections.Generic;
using GameEngine.Models;
using GameEngine.Presentation;

namespace GameEngine.GameLogic
{
    public class NewGame
    {
        public static bool CreateNewGame()
        {
            Console.Clear();
            var selectedOption = Menu.ShowMenu("Select the number of players:", new List<string> { "2", "3", "4" });
            int numberOfPlayers = 2;
            switch (selectedOption)
            {
                case 0:
                    break;

                case 1:
                    numberOfPlayers = 3;
                    break;

                case 2:
                    numberOfPlayers = 4;
                    break;
            }

            Console.WriteLine("Enter name of board:");
            var boardName = Console.ReadLine();
            var board = GameFactory.CreateBoard(boardName); // Return new board
            board.Players = AddPlayers(numberOfPlayers, board); // Add all players to list
            bool exit = Action.StartGame(board); // Start Game and wait for exit to return
            return exit;
        }

        private static List<Player> AddPlayers(int numberOfPlayers, Board board) // Get names of all players and colors of tokens from user
        {
            var colorsListString = Utility.GetListOfColorsString();
            var players = new List<Player>();
            for (int i = 1; i <= numberOfPlayers; i++)
            {
                Player p = AddOnePlayer(i, colorsListString, board);
                players.Add(p);
                string selectedColor = p.Tokens[0].Color.ToString();
                colorsListString.Remove(selectedColor);
            }
            return players;
        }

        private static Player AddOnePlayer(int playerNumber, List<string> colors, Board board) // Get name of one player and color of tokens from user
        {
            Console.Clear();
            Console.Write($"Enter name of {playerNumber} player: ");
            string name = Console.ReadLine();

            var selectedOption = Menu.ShowMenu("Choose color of the tokens:", colors);
            string selectedColorString = colors[selectedOption];
            TokenColor selectedColorEnum = Utility.ColorFromStringToEnum(selectedColorString);

            var player = GameFactory.NewPlayer(name, board, selectedColorEnum);
            return player;
        }
    }
}