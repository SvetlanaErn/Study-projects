using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Models;

namespace GameEngine
{
    public class Utility
    {
        public static void PrintInfo(Player player) // Print player info in GUI format
        {
            Console.WriteLine();
            Console.WriteLine("|  N  |   Square   |  Active  ");
            Console.WriteLine("------------------------------");
            var tokensList = GetTokensList(player);
            foreach (var t in tokensList)
            {
                Console.WriteLine(t);
            }
        }

        public static List<string> GetTokensList(Player player) // GUI for players' tokens
        {
            List<string> tokensList = new List<string>();
            for (int i = 0; i < player.Tokens.Count; i++)
            {
                tokensList.Add($"|  {i + 1}  |    {player.Tokens[i].Route[player.Tokens[i].Steps]}    |   {player.Tokens[i].IsActive}");
            }
            return tokensList;
        }

        public static TokenColor ColorFromStringToEnum(string selectedColorString)
        {
            var colorsArrayEnum = Enum.GetValues(typeof(TokenColor)); // Array of colors
            return colorsArrayEnum.Cast<TokenColor>().FirstOrDefault(c => c.ToString() == selectedColorString); // Return the color that matches input color
        }

        public static List<string> GetListOfColorsString() // return List<string> ("Blue", "Yellow","Red", "Green")
        {
            var colorsArrayEnum = Enum.GetValues(typeof(TokenColor));
            return (from object c in colorsArrayEnum select c.ToString()).ToList();
        }
    }
}