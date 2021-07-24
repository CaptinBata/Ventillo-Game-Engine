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
        private double LastTime = 0;
        private double FPS = 0;

        public DebugObject(Vector position, MinMax playableArea) : base(position)
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
                ),
                new DrawObject(
                    new List<Vector>(){
                        new Vector(playableArea.Max.x + 1, playableArea.Min.y),
                        new Vector(playableArea.Max.x + 1, playableArea.Max.y),
                        new Vector(playableArea.Max.x - 1, playableArea.Max.y),
                        new Vector(playableArea.Max.x - 1, playableArea.Min.y),
                    },
                    new Color(Convert.ToByte(209),Convert.ToByte(49),Convert.ToByte(17),Convert.ToByte(0.45)),
                    new Color(Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0))
                ),
                new DrawObject(
                    new List<Vector>(){
                        new Vector(playableArea.Max.x, playableArea.Max.y - 1),
                        new Vector(playableArea.Max.x, playableArea.Max.y + 1),
                        new Vector(playableArea.Min.x, playableArea.Max.y + 1),
                        new Vector(playableArea.Min.x, playableArea.Max.y - 1),
                        },
                    new Color(Convert.ToByte(209),Convert.ToByte(49),Convert.ToByte(17),Convert.ToByte(0.45)),
                    new Color(Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0))
                ),
                new DrawObject(
                    new List<Vector>(){
                        new Vector(xMidPosition - 1, playableArea.Min.y),
                        new Vector(xMidPosition + 1, playableArea.Min.y),
                        new Vector(xMidPosition + 1, playableArea.Max.y),
                        new Vector(xMidPosition - 1, playableArea.Max.y),
                        },
                    new Color(Convert.ToByte(209),Convert.ToByte(49),Convert.ToByte(17),Convert.ToByte(0.45)),
                    new Color(Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0))
                ),
                new DrawObject(
                    new List<Vector>(){
                        new Vector(playableArea.Min.x, yMidPosition - 1),
                        new Vector(playableArea.Max.x, yMidPosition - 1),
                        new Vector(playableArea.Max.x, yMidPosition + 1),
                        new Vector(playableArea.Min.x, yMidPosition + 1),
                        },
                    new Color(Convert.ToByte(209),Convert.ToByte(49),Convert.ToByte(17),Convert.ToByte(0.45)),
                    new Color(Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0))
                ),
            };

            this.SetDrawObject(DrawObjects);
        }

        public double GetFPS()
        {
            return this.FPS;
        }

        private void UpdateFPS(double timestamp)
        {
            if (this.LastTime != 0)
            {
                var timeTaken = timestamp - this.LastTime;
                this.FPS = 1000 / timeTaken;
            }

            this.LastTime = timestamp;
        }

        public void Draw()
        {
            this.DrawByLine(this.DrawObjects);

            this.DrawByText(
                $"FPS {this.FPS}",
                new Vector(Engine.GetWindowWidth() * 0.96, Engine.GetWindowHeight() * 0.99),
                new Color(Convert.ToByte(209), Convert.ToByte(49), Convert.ToByte(17), Convert.ToByte(0.45))
            );
        }
    }
}