using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Shop
{
    public class Product
    {
        public string Article;
        public string Name;
        public string Description;
        public decimal Price;
    }
    public class Program
    {
        public static List<Product> productList;    // List with assortment of the shop
        public static string[] productArray;    // Array with assortment of the shop prepaid for display on the console
        public static Dictionary<Product, int> cart; // Cart <product, quantity>
        public const string pathProductRange = "ProductRange.csv"; // address of the file with shops assortment
        public const string pathCart = @"C:\Windows\Temp\Cart_ProgramButik.txt"; // address of the file with cart
        public static void Main()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            LoadProductRange();     // Loading shops assortment from the file
            CreateProductArray();   // Prepairing array of string for display 
            LoadCart();     // Loading the cart if it exists

            while (true)
            {
                ShowProductRange(); // Show shops assortment

                int selection = ShowMenu("Vad vill du göra?", new[]
                {
                    "Lägg till produkt",
                    "Ta bort produkt",
                    "Visa varukorg",
                    "Beställ",
                    "Spara varukorg för att fortsätta handla senare",
                    "Exit"
                });
                Console.Clear();

                if (selection == 0)
                {
                    AddProduct();
                }
                else if (selection == 1)
                {
                    RemoveProduct();
                }
                else if (selection == 2)
                {
                    ShowCart("Din varukorg:");
                }
                else if (selection == 3)
                {
                    PlaceOrder();
                }
                else if (selection == 4)
                {
                    SaveCart();
                    break;
                }
                else if (selection == 5)
                {
                    break;
                }
            }
            Console.WriteLine(Environment.NewLine + "Välkommen åter!");
        }
        public static void LoadProductRange()
        {
            if (!File.Exists(pathProductRange))
            {
                Console.WriteLine("Filen med sortiment finns inte!");
                Environment.Exit(1);
            }

            productList = new List<Product>();

            string[] contents = File.ReadAllLines(pathProductRange);
            foreach (string s in contents)
            {
                try
                {
                    string[] words = s.Split(',');
                    Product p = new Product
                    {
                        Article = words[0],
                        Name = words[1],
                        Description = words[2],
                        Price = decimal.Parse(words[3])
                    };
                    productList.Add(p);
                }
                catch
                {
                    Console.WriteLine("Fel vid inläsning av en produkt!");
                }
            }
        }
        public static void CreateProductArray()
        {
            productArray = new string[productList.Count];
            int i = 0;
            foreach (Product p in productList)
            {
                productArray[i] = p.Article + ": " + p.Name + " - " + p.Description + "(" + p.Price + " kr)";
                i++;
            }
        }
        // Load the cart if it exists 
        public static void LoadCart()
        {
            cart = new Dictionary<Product, int>();

            if (File.Exists(pathCart))
            {
                string[] contentsCart = File.ReadAllLines(pathCart);
                foreach (string s in contentsCart)
                {
                    string[] words = s.Split(',');
                    cart.Add(productList.Single(p => p.Article == words[0]), int.Parse(words[1]));
                }
                ShowCart("Din varukorg:");
            }
        }
        public static void ShowProductRange()
        {
            Console.WriteLine("Följande produkter finns i butiken:" + Environment.NewLine);
            foreach (string s in productArray)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine();
        }
        public static void ShowCart(string prompt)
        {

            if (cart.Count == 0)
            {
                Console.WriteLine("Din varukorg är tom.");
            }
            else
            {
                string[] cartToShow = CartToArray();
                Console.WriteLine(prompt);
                foreach (string s in cartToShow)
                {
                    Console.WriteLine(s);
                }
            }
            Console.WriteLine();
        }
        public static string[] CartToArray()
        {
            var temp = new List<string>();
            foreach (KeyValuePair<Product, int> p in cart)
            {
                temp.Add("-" + p.Key.Name + ": " + p.Value);
            }
            return temp.ToArray();
        }
        public static void SaveCart()
        {
            string[] cartString = new string[cart.Count];

            int j = 0;
            foreach (KeyValuePair<Product, int> pair in cart)
            {
                cartString[j] = pair.Key.Article + "," + pair.Value;
                j++;
            }
            File.WriteAllLines(pathCart, cartString);
            Console.WriteLine("Din varukorg sparas." + Environment.NewLine);
        }
        public static void AddProduct()
        {
            int indexProduct = ShowMenu("Välj produkt:", productArray);
            Product selected = productList[indexProduct];
            Console.Clear();
            Console.WriteLine(productArray[indexProduct]);
            Console.Write("Ange antal att köpa: ");
            int quantity = int.Parse(Console.ReadLine());
            Console.WriteLine();

            if (cart.ContainsKey(selected))
            {
                cart[selected] += quantity;
            }
            else
            {
                cart[selected] = quantity;
            }

            Console.WriteLine(quantity + " exemplar av " + productList[indexProduct].Name + " har lagts till i varukorgen" + Environment.NewLine);
        }
        public static void RemoveProduct()
        {
            string[] temp = CartToArray();
            int indexProduct = ShowMenu("Välj produkt för att ta bort:", temp);
            string selectedString = temp[indexProduct];
            int index = selectedString.LastIndexOf(':');
            string selectedName = selectedString.Remove(index).Substring(1);
            var selectedProduct = cart.Single(p => p.Key.Name == selectedName);
            cart.Remove(selectedProduct.Key);
            Console.Clear();
            Console.WriteLine(selectedProduct.Key.Name + " blev borttagen från varukorgen." + Environment.NewLine);
        }
        // Show the cart and total sum. Clear the cart, delete the file with the cart if exists
        public static void PlaceOrder()
        {
            ShowCart("Du har lagt följande beställning:");
            Console.WriteLine("Totalsumma: " + SumCart());
            Console.WriteLine("Tack för din beställning!" + Environment.NewLine);
            cart.Clear();
            if (File.Exists(pathCart)) File.Delete(pathCart);
        }
        // Calculates the sum of products in the cart
        public static decimal SumCart()
        {
            decimal sum = 0;
            foreach (KeyValuePair<Product, int> pair in cart)
            {
                sum += pair.Value * pair.Key.Price;
            }
            return sum;
        }
        public static int ShowMenu(string prompt, string[] options)
        {
            if (options == null || options.Length == 0)
            {
                throw new ArgumentException("Cannot show a menu for an empty array of options.");
            }

            Console.WriteLine(prompt);

            int selected = 0;

            // Hide the cursor that will blink after calling ReadKey.
            Console.CursorVisible = false;

            ConsoleKey? key = null;
            while (key != ConsoleKey.Enter)
            {
                // If this is not the first iteration, move the cursor to the first line of the menu.
                if (key != null)
                {
                    Console.CursorLeft = 0;
                    Console.CursorTop = Console.CursorTop - options.Length;
                }

                // Print all the options, highlighting the selected one.
                for (int i = 0; i < options.Length; i++)
                {
                    var option = options[i];
                    if (i == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine("- " + option);
                    Console.ResetColor();
                }

                // Read another key and adjust the selected value before looping to repeat all of this.
                key = Console.ReadKey().Key;
                selected = key switch
                {
                    ConsoleKey.DownArrow => Math.Min(selected + 1, options.Length - 1),
                    ConsoleKey.UpArrow => Math.Max(selected - 1, 0),
                    _ => selected
                };
            }

            // Reset the cursor and return the selected option.
            Console.CursorVisible = true;
            return selected;
        }
    }
}
