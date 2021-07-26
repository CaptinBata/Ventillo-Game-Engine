using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ventillo;
using Ventillo.System;
using Ventillo.GameObjects;

namespace Example.GameObjects
{
    class Square : GameObject
    {
        public Square(Vector position) : base(position)
        {
            var drawObject = new List<DrawObject>(){
                new DrawObject(
                    new List<Vector>()
                    {
                        new Vector(-50, -50),
                        new Vector(50, -50),
                        new Vector(50, 50),
                        new Vector(-50, 50),
                    },
                    new Color(110, 34, 199, 255),
                    new Color()
                )
            };

            SetDrawObject(drawObject);
        }

        public override void Update(List<GameObject> gameObjects)
        {
            for (var keyIndex = 0; keyIndex < Engine.keys.Count; keyIndex++)
            {
                var key = Engine.keys.ElementAt(keyIndex);

                switch (key)
                {
                    case "W":
                        Position.Translate(new Vector(0, -10));
                        Engine.keys.Remove(key);
                        break;
                    case "A":
                        Position.Translate(new Vector(-10, 0));
                        Engine.keys.Remove(key);
                        break;
                    case "S":
                        Position.Translate(new Vector(0, 10));
                        Engine.keys.Remove(key);
                        break;
                    case "D":
                        Position.Translate(new Vector(10, 0));
                        Engine.keys.Remove(key);
                        break;
                }
            }
        }
    }
}
