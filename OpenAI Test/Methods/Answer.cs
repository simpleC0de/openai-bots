using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenAI_Test.Methods
{
    class Answer
    {
        static bool stop = false;

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
                Task task = CompletionRequest.createApiRequest(api_key, Console.ReadLine().Replace("Ich: ", ""), Program.getConfig());
                while (!task.IsCompleted)
                    Program.block();
                //Console.WriteLine("Task complete");

                System.GC.Collect();
            }
        }
    }
}
