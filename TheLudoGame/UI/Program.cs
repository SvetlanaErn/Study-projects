using System;
using System.Collections.Generic;
using GameEngine.DataAccess;
using GameEngine.GameLogic;
using GameEngine.Presentation;

namespace UI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MainMenu();
        }

        private static void MainMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                var selectedOption = Menu.ShowMenu("LUDO GAME\n", new List<string>
                {
                    "Start a new game",
                    "Load a saved game",
                    "Exit"
                });

                switch (selectedOption)
                {
                    case 0:
                        exit = NewGame.CreateNewGame();
                        break;

                    case 1:
                        GameState.LoadSavedGame();
                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine("Goodbye!");
                        exit = true;
                        break;
                }
            }
        }
    }
}