using System;
using System.Drawing;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace ConsoleShutdownTimer
{
    /// <summary>
    /// Prints welcome banner and requests User input.
    /// </summary>
    internal class Program
    {
        private static int TotalSeconds { get; set; }
        private static int currentLineCursorTop { get; set; }
        private static int currentLineCursorLeft { get; set; }

        private static void Main(string[] args)
        {
            Console.WriteAscii("SHUTDOWN", Color.ForestGreen);
            Console.WriteLine("Please enter requested wait until system shutdown.", Color.IndianRed);
            Console.Write("Hours: ", Color.IndianRed);

            var Hours = Console.ReadLine().Replace("Hours: ", "");
            int hh;
            while (!Int32.TryParse(Hours, out hh))
            {
                Console.WriteLine("Not a valid number, try again.", Color.IndianRed);
                Console.Write("Hours: ", Color.IndianRed);
                Hours = Console.ReadLine().Replace("Hours: ", "");
            }
            Console.Write("Minutes: ", Color.IndianRed);

            var Minutes = Console.ReadLine().Replace("Minutes: ", "");
            int mm;
            while (!Int32.TryParse(Minutes, out mm))
            {
                Console.WriteLine("Not a valid number, try again.", Color.IndianRed);
                Console.Write("Minutes: ", Color.IndianRed);
                Minutes = Console.ReadLine().Replace("Minutes: ", "");
            }

            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");

            TotalSeconds = (hh * 3600) + (mm * 60);
            Console.WriteLine("Cancel shutdown with Esc. ", Color.IndianRed);
            Console.Write("Time Left: ", Color.IndianRed);
            currentLineCursorTop = Console.CursorTop;
            currentLineCursorLeft = Console.CursorLeft;
            System.Diagnostics.Process.Start("shutdown", "/s /t " + TotalSeconds);
            Timer();
            ListenForEscape();
        }

        public static void WriteTimeLeft()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(currentLineCursorLeft, currentLineCursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(currentLineCursorLeft, currentLineCursorTop);
            Console.Write(TotalSeconds);
            Console.SetCursorPosition(currentLineCursorLeft, currentLineCursorTop);
            Console.CursorVisible = true;
            TotalSeconds -= 1;
        }

        public static void Timer()
        {
            for (int i = TotalSeconds; i > 0; i--)
            {
                var t = Task.Delay(i * 1000).ContinueWith(_ => WriteTimeLeft());
            }
        }

        public static void ListenForEscape()
        {
            while (Console.ReadKey().Key == ConsoleKey.Escape)
            {
                System.Diagnostics.Process.Start("shutdown", "/a");
                Environment.Exit(0);
            }
        }
    }
}