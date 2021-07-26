using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ventillo.GameObjects;
using Ventillo.Utils;

namespace Example.GameObjects
{
    class Square : GameObject
    {
        public Square(Vector position)
        {
            var drawObject = new List<DrawObject>(){
                new DrawObject(
                    new List<Vector>()
                    {
                        new Vector(Position.x-5,Position.y-5),
                        new Vector(Position.x+5,Position.y-5),
                        new Vector(Position.x+5,Position.y+5),
                        new Vector(Position.x-5,Position.y+5),
                    },
                    new SFML.Graphics.Color(Convert.ToByte(110), Convert.ToByte(34), Convert.ToByte(199), Convert.ToByte(1)),
                    new SFML.Graphics.Color()
                )
            };

            SetDrawObject(drawObject);
        }
    }
}
