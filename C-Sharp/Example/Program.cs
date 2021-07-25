using System;
using System.Collections.Generic;
using Ventillo;
using Ventillo.GameObjects;
using Ventillo.Interfaces;

namespace Example
{
    class Example : IGame
    {
        public List<GameObject> gameObjects = new List<GameObject>();

        protected override void CheckDelete()
        {

        }

        public override void Draw()
        {

        }

        public override void Update()
        {

        }
    }
    class Program
    {
        static Engine engine = new Engine(60);
        static Example exampleGame = new Example();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            engine.SetGame(exampleGame);
            engine.Start();
        }
    }
}
