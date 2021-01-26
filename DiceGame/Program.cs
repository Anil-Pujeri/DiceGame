using System;
using DiceGameClassLibrary;

namespace DiceGame
{
    class Program
    {
        static void Main(string[] args)
        {
            (int N, int M) = ReadCommandLineArgs(args);
            var game = new Game(N,M, OutputCallback, InputCallback);
            game.Play();
            Console.ReadLine();
        }

        static (int, int) ReadCommandLineArgs(string[] args)
        {
            if(args.Length !=2)
            {
                throw new ArgumentException("Please provide the values for N and M in the command line arguments");
            }

            int N = Convert.ToInt32(args[0]);
            int M = Convert.ToInt32(args[1]);

            return (N, M);

        }

        static void OutputCallback(string text)
        {
            Console.WriteLine(text);
        }

        static char InputCallback()
        {
           var keyPressed = Console.ReadKey(true);
            return keyPressed.KeyChar;
        }
    }
}
