using System;
using System.Collections.Generic;
using Ventillo;
using Ventillo.GameObjects;
using Ventillo.Interfaces;

using Example.Game;

namespace Example
{
    class Program
    {
        static Engine engine = new Engine(60);
        static ExampleGame exampleGame = new ExampleGame();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            engine.SetGame(exampleGame);
            engine.Start();
        }
    }
}
