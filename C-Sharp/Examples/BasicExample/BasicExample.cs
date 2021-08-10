using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ventillo;
using Ventillo.System;
using Ventillo.Interfaces;

using Example.GameObjects;

namespace Example.Game
{
    internal class ExampleGame : IGame
    {

        internal ExampleGame()
        {
            Engine.SetWindowTitle("Example Game");

            Engine.logger.Info("Test");
            Engine.logger.Debug("Test");
            Engine.logger.Warn("Test");
            Engine.logger.Error("Test");

            Engine.logger.Info("Creating Square Object");
            var square = new Square(new Vector(Engine.playableArea.Max.x / 2, Engine.playableArea.Max.y / 2));
            Engine.logger.Info("Created Square Object", square);
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
