using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;

using Ventillo.GameObjects;
using Ventillo.Utils;

namespace Ventillo.Debug
{
    class DebugObject : GameObject
    {
        private int LastTime = 0;
        private int FPS = 0;

        DebugObject(Vector position, MinMax playableArea) : base(position)
        {
            double xMidPosition = playableArea.Min.x + ((playableArea.Max.x - playableArea.Min.x) / 2);
            double yMidPosition = playableArea.Min.y + ((playableArea.Max.y - playableArea.Min.y) / 2);

            List<DrawObject> DrawObjects = new List<DrawObject>()
            {
                new DrawObject(
                    new List<Vector>(){
                        new Vector(playableArea.Min.x - 1, playableArea.Min.y),
                        new Vector(playableArea.Min.x + 1, playableArea.Min.y),
                        new Vector(playableArea.Min.x + 1, playableArea.Max.y),
                        new Vector(playableArea.Min.x - 1, playableArea.Max.y),
                    },
                    new Color(Convert.ToByte(209),Convert.ToByte(49),Convert.ToByte(17),Convert.ToByte(0.45)),
                    new Color(Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0))
                ),
                new DrawObject(
                    new List<Vector>(){
                        new Vector(playableArea.Min.x, playableArea.Min.y - 1),
                        new Vector(playableArea.Max.x, playableArea.Min.y - 1),
                        new Vector(playableArea.Max.x, playableArea.Min.y + 1),
                        new Vector(playableArea.Min.x, playableArea.Min.y + 1),
                    },
                    new Color(Convert.ToByte(209),Convert.ToByte(49),Convert.ToByte(17),Convert.ToByte(0.45)),
                    new Color(Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0))
                )
            };

            this.SetDrawObject(DrawObjects);
        }
    }
}