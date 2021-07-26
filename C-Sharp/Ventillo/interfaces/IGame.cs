using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventillo.GameObjects;

namespace Ventillo.Interfaces
{
    public class IGame
    {
        public List<GameObject> gameObjects = new List<GameObject>();
        protected virtual void CheckDelete()
        {
            for (var gameObjectIndex = 0; gameObjectIndex < gameObjects.Count; gameObjectIndex++)
            {
                var gameObject = gameObjects.ElementAt(gameObjectIndex);

                if (gameObject.ToDelete)
                {
                    DeleteGameObject(gameObject);
                }
            }
        }
        public virtual void Update()
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Update(gameObjects);
            }
        }
        public virtual void Draw()
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Draw();
            }
        }

        protected void DeleteGameObject(GameObject gameObject)
        {
            gameObject = null;
            gameObjects.Remove(gameObject);
        }

    }
}