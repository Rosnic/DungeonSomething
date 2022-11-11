using System;
using System.Threading;
using SadConsole;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace DungeonSomething
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            System.Console.WriteLine("I am writing to the console before deletion!");

            // Creates the initial console
            Game.Create(150, 45);
            Settings.WindowTitle = "DungeonSomething H2";

            Game.Instance.OnStart = Init; //  Callback to the run method

            Game.Instance.Run();

            Game.Instance.Dispose();
        }

        private static void Init()
        {
            Game.Instance.Screen = new RootScreen();
            Game.Instance.Screen.IsFocused = true; // If Focused, it is the surface/console, on which the keyboard and mouse operates
            Game.Instance.DestroyDefaultStartingConsole(); //  Removes the default console
        }
    }
}