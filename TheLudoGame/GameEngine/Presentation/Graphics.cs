using System;
using System.Linq;
using GameEngine.Models;

namespace GameEngine.Presentation
{
    public static class Graphics
    {
        public static void Draw(Board board) // Not finished
        {
            int left = 21;
            int top = 0;
            int leaderTop = 0;

            Console.SetCursorPosition(114, 1);
            Console.WriteLine("Leaderboard");

            foreach (var player in board.Players.OrderBy(p => p.Tokens.Count).ThenBy(p => p.Tokens.Average(s => s.Steps))) // Calculate leader by number of tokens left and then by avg amout of steps taken by tokens.
            {
                Console.SetCursorPosition(114, 2 + leaderTop);
                Console.Write($"{player.Name} | Tokens Left: {player.Tokens.Count} ");
                leaderTop++;
            }

            for (int i = 0; i < board.Squares.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;

                if (board.Squares[i].Occupants.Count != 0) // Set the correct color for the square
                {
                    var color = ConsoleColor.White;
                    switch (board.Squares[i].Occupants[0].Color)
                    {
                        case TokenColor.Blue:
                            color = ConsoleColor.Blue;
                            break;

                        case TokenColor.Green:
                            color = ConsoleColor.Green;
                            break;

                        case TokenColor.Red:
                            color = ConsoleColor.Red;
                            break;

                        case TokenColor.Yellow:
                            color = ConsoleColor.Yellow;
                            break;
                    }

                    Console.ForegroundColor = color;
                }
                Console.SetCursorPosition(80, i);

                if (i < 5) // Spaghetti code for drawing the board
                {
                    Console.SetCursorPosition(80, 3 + top);
                    top++;
                }

                if (i >= 5 && i < 12)
                {
                    Console.SetCursorPosition(59 + left, 8);
                    left += 4;
                    top = 0;
                }

                if (i == 12)
                {
                    Console.SetCursorPosition(104, 9);
                    left = 0;
                }

                if (i == 13)
                {
                    Console.SetCursorPosition(104, 10);
                }

                if (i >= 14 && i <= 19)
                {
                    Console.SetCursorPosition(100 - left, 10);
                    left += 4;
                }

                if (i > 19 && i < 26)
                {
                    Console.SetCursorPosition(80, 11 + top);
                    top++;
                }

                if (i == 26)
                {
                    Console.SetCursorPosition(76, 16);
                    top = 0;
                    left = 0;
                }

                if (i >= 27 && i <= 33)
                {
                    Console.SetCursorPosition(72, 16 - top);
                    top++;
                }

                if (i > 33 && i <= 39)
                {
                    Console.SetCursorPosition(68 - left, 10);
                    left += 4;
                }
                if (i == 40)
                {
                    Console.SetCursorPosition(48, 9);
                    top = 0;
                    left = 0;
                }
                if (i >= 41 && i <= 47)
                {
                    Console.SetCursorPosition(48 + left, 8);
                    left += 4;
                }
                if (i > 47 && i <= 53)
                {
                    Console.SetCursorPosition(72, 7 - top);
                    top++;
                    left = 0;
                }
                if (i >= 54 && i <= 55)
                {
                    Console.SetCursorPosition(76 + left, 2);
                    left += 4;
                    top = 0;
                }

                if (i >= 56 && i <= 61)
                {
                    Console.SetCursorPosition(76, 3 + top);
                    Console.WriteLine($"[x]");
                    top++;
                    left = 0;
                    continue;
                }
                if (i >= 62 && i <= 67)
                {
                    Console.SetCursorPosition(100 - left, 9);
                    Console.WriteLine($"[x]");
                    left += 4;
                    top = 0;
                    continue;
                }

                if (i >= 68 && i <= 73)
                {
                    Console.SetCursorPosition(76, 15 - top);
                    Console.WriteLine($"[x]");
                    left = 0;
                    top++;
                    continue;
                }

                if (i >= 74 && i <= 79)
                {
                    Console.SetCursorPosition(52 + left, 9);
                    Console.WriteLine($"[x]");
                    left += 4;
                    continue;
                }


                if (i < 10)
                {
                    Console.WriteLine($"[0{i}]");
                    continue;
                }
                Console.WriteLine($"[{i}]");
            }

            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}