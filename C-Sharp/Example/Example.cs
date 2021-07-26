using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ventillo;
using Ventillo.Utils;
using Ventillo.Interfaces;

using Example.GameObjects;

namespace Example.Game
{
    internal class ExampleGame : IGame
    {

        internal ExampleGame()
        {
            var square = new Square(new Vector(Engine.playableArea.Max.x / 2, Engine.playableArea.Max.y / 2));
            gameObjects.Add(square);
        }

        ~ExampleGame()
        {
            for (var gameObjectIndex = 0; gameObjectIndex < gameObjects.Count; gameObjectIndex++)
            {
                var gameObject = gameObjects.ElementAt(gameObjectIndex);

                DeleteGameObject(gameObject);
            }
        }
    }
}
