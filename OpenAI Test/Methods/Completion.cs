using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OpenAI_Test.Methods
{
    class Completion
    {

        private static bool stop = false;

        public static async Task startAsync(string api_key)
        {
            // loop until user stops
            Console.Clear();
            Console.WriteLine("Press ESC at any time to stop");
            Thread.Sleep(2500);

            // Monitor user key presses on a second thread and stop if escape is pressed
            Thread t = new Thread(delegate ()
            {
                while (true)
                {
                    if (Console.ReadKey().Key == ConsoleKey.Escape)
                    {
                        stop = true;
                        Console.Clear();
                        Program.displayOptions();
                        Thread.CurrentThread.Abort();
                    }
                }
            });

            t.Start();


            while (!stop)
            {
                Console.Write("Ich: ");
                Task task =  WebRequest.createCompletionAPIRequest(api_key, Console.ReadLine().Replace("Ich: ", ""));
                while (!task.IsCompleted)
                    Program.block();
                //Console.WriteLine("Task complete");

                System.GC.Collect();
            }
        }

    }
}
