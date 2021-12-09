using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenAI_Test.Methods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OpenAI_Test
{
    class Program
{

        static bool inMenu = true;
        static string api_key;
        // Convert objects to the correct datatypes before use
        static Dictionary<string, object> config;

        static void Main(string[] args)
        {
            // Reading and initializing config file
            config = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(Environment.CurrentDirectory + "\\config.json"));

            if ((config["api_key"].ToString().Length < 5))
            {
                Console.WriteLine("Paste ur OpenAI Api Key now");
                api_key = Console.ReadLine();
                Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine($"Found API Key: {config["api_key"]}\nPress any key to continue");
                Console.ReadKey();

                api_key = config["api_key"].ToString();
            }

            displayOptions();

            Console.ReadLine();
        }
        // private api_key = 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
        private static async Task startProgramAsync(int sel)
        {
            Console.Clear();

            switch (sel)
            {
                case 0:
                    Completion.startAsync(api_key);
                   break;
                case 1:
                    //Console_Chatbot.start();
                    Console.WriteLine("Application exit 1");
                    break;
                case 2:
                    Console.WriteLine("Theoretic config reload");
                    //Environment.Exit(0);
                    break;
                case 3:
                    Environment.Exit(69);
                    break;
                default:
                    Console.WriteLine("Application exit 3");
                    break;

            }
        }

        static int selIndex = 0;

        static Dictionary<int, string> options = new Dictionary<int, string>
            {
                {0, "Completion"},
                {1, "Answer"},
                {2, "Reload config" },
                {3, "Exit"}
            };

        public static bool block()
        {
            Thread.Sleep(5);
            return true;
        }

        public static void displayOptions()
        {
            selIndex = 0;
            Console.Clear();
            do
            {
                Console.SetCursorPosition(0, 0);
                foreach (KeyValuePair<int, string> kvp in options)
                {
                    if (selIndex == kvp.Key)
                        Console.WriteLine("->" + kvp.Value);
                    else
                        Console.WriteLine("  " + kvp.Value);
                }

                ConsoleKey pressedKey = Console.ReadKey().Key;
                if (pressedKey == ConsoleKey.DownArrow)
                    if (selIndex < options.Count - 1)
                        selIndex += 1;
                if (pressedKey == ConsoleKey.UpArrow)
                    if (selIndex > 0)
                        selIndex -= 1;
                if (pressedKey == ConsoleKey.Enter)
                {
                    inMenu = false;
                    startProgramAsync(selIndex);
                }

                // Debugging
                //Console.WriteLine("Pressed key: " + pressedKey.ToString() + " | Index: " + selIndex);

            } while (inMenu);

        }


    }
}
