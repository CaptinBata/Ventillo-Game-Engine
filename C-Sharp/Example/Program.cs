using System;
using System.Collections.Generic;
using Ventillo;
using Ventillo.GameObjects;
using Ventillo.Interfaces;

namespace Example
{
    class Example : IGame
    {
        public List<GameObject> gameObjects { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void checkDelete()
        {
            throw new NotImplementedException();
        }

        public void draw()
        {
            throw new NotImplementedException();
        }

        public void update()
        {
            throw new NotImplementedException();
        }
    }
    class Program
    {
        static Engine engine = new Engine();
        static Example exampleGame = new Example();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            engine.SetGame(exampleGame);
            engine.Start();
        }
    }
}
