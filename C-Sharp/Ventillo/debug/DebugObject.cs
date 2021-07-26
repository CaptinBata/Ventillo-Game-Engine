using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;
using SFML.System;

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
                    new Color(Convert.ToByte(209),Convert.ToByte(49),Convert.ToByte(17),Convert.ToByte(155)),
                    new Color()
                ),
                new DrawObject(
                    new List<Vector>(){
                        new Vector(playableArea.Min.x, playableArea.Min.y - 1),
                        new Vector(playableArea.Max.x, playableArea.Min.y - 1),
                        new Vector(playableArea.Max.x, playableArea.Min.y + 1),
                        new Vector(playableArea.Min.x, playableArea.Min.y + 1),
                    },
                    new Color(Convert.ToByte(209),Convert.ToByte(49),Convert.ToByte(17),Convert.ToByte(155)),
                    new Color()
                ),
                new DrawObject(
                    new List<Vector>(){
                        new Vector(playableArea.Max.x + 1, playableArea.Min.y),
                        new Vector(playableArea.Max.x + 1, playableArea.Max.y),
                        new Vector(playableArea.Max.x - 1, playableArea.Max.y),
                        new Vector(playableArea.Max.x - 1, playableArea.Min.y),
                    },
                    new Color(Convert.ToByte(209),Convert.ToByte(49),Convert.ToByte(17),Convert.ToByte(115)),
                    new Color()
                ),
                new DrawObject(
                    new List<Vector>(){
                        new Vector(playableArea.Max.x, playableArea.Max.y - 1),
                        new Vector(playableArea.Max.x, playableArea.Max.y + 1),
                        new Vector(playableArea.Min.x, playableArea.Max.y + 1),
                        new Vector(playableArea.Min.x, playableArea.Max.y - 1),
                        },
                    new Color(Convert.ToByte(209),Convert.ToByte(49),Convert.ToByte(17),Convert.ToByte(115)),
                    new Color()
                ),
                new DrawObject(
                    new List<Vector>(){
                        new Vector(xMidPosition - 1, playableArea.Min.y),
                        new Vector(xMidPosition + 1, playableArea.Min.y),
                        new Vector(xMidPosition + 1, playableArea.Max.y),
                        new Vector(xMidPosition - 1, playableArea.Max.y),
                        },
                    new Color(Convert.ToByte(209),Convert.ToByte(49),Convert.ToByte(17),Convert.ToByte(115)),
                    new Color()
                ),
                new DrawObject(
                    new List<Vector>(){
                        new Vector(playableArea.Min.x, yMidPosition - 1),
                        new Vector(playableArea.Max.x, yMidPosition - 1),
                        new Vector(playableArea.Max.x, yMidPosition + 1),
                        new Vector(playableArea.Min.x, yMidPosition + 1),
                        },
                    new Color(Convert.ToByte(209),Convert.ToByte(49),Convert.ToByte(17),Convert.ToByte(115)),
                    new Color()
                ),
            };

            this.SetDrawObject(DrawObjects);
        }

        public double GetFPS()
        {
            return this.FPS;
        }

        internal void UpdateFPS(double timestamp)
        {
            if (this.LastTime != 0)
            {
                var timeTaken = timestamp - this.LastTime;
                this.FPS = 1000 / timeTaken;
            }

            this.LastTime = timestamp;
        }

        public override void Draw()
        {
            this.DrawByLine(this.DrawObjects);

            this.DrawByText(
                $"FPS {this.FPS}",
                new Vector(Engine.GetWindowWidth() * 0.95, Engine.GetWindowHeight() * 0.98),
                new Color(Convert.ToByte(209), Convert.ToByte(49), Convert.ToByte(17), Convert.ToByte(115))
            );
        }

        public void DrawObjectBounds(GameObject objectToDraw)
        {
            var minGlobal = objectToDraw.ToGlobalCoords(objectToDraw.MinMax.Min);
            var maxGlobal = objectToDraw.ToGlobalCoords(objectToDraw.MinMax.Max);

            ConvexShape TempShape = new ConvexShape();
            TempShape.Position = new Vector2f().UseVentilloVector(objectToDraw.Position);
            TempShape.SetPointCount(4);

            TempShape.SetPoint(0, new Vector2f().UseVentilloVector(minGlobal));
            TempShape.SetPoint(1, new Vector2f().UseVentilloVector(new Vector(maxGlobal.x, minGlobal.y)));
            TempShape.SetPoint(2, new Vector2f().UseVentilloVector(maxGlobal));
            TempShape.SetPoint(3, new Vector2f().UseVentilloVector(new Vector(minGlobal.x, maxGlobal.y)));

            this.setDrawModes(TempShape, new Color(), new Color(Convert.ToByte(209), Convert.ToByte(49), Convert.ToByte(17), Convert.ToByte(115)));
            Engine.window.Draw(TempShape);

            for (var drawableIndex = 0; drawableIndex < objectToDraw.DrawObjects.Count; drawableIndex++)
            {
                var drawable = objectToDraw.DrawObjects.ElementAt(drawableIndex);

                var minObjGlobal = objectToDraw.ToGlobalCoords(drawable.MinMax.Min);
                var maxObjGlobal = objectToDraw.ToGlobalCoords(drawable.MinMax.Max);

                ConvexShape TempShapeObj = new ConvexShape();
                TempShapeObj.Position = new Vector2f().UseVentilloVector(objectToDraw.Position);
                TempShapeObj.SetPointCount(4);

                TempShapeObj.SetPoint(0, new Vector2f().UseVentilloVector(minObjGlobal));
                TempShapeObj.SetPoint(1, new Vector2f().UseVentilloVector(new Vector(maxObjGlobal.x, minObjGlobal.y)));
                TempShapeObj.SetPoint(2, new Vector2f().UseVentilloVector(maxObjGlobal));
                TempShapeObj.SetPoint(3, new Vector2f().UseVentilloVector(new Vector(minObjGlobal.x, maxObjGlobal.y)));

                this.setDrawModes(TempShapeObj, new Color(), new Color(Convert.ToByte(209), Convert.ToByte(49), Convert.ToByte(17), Convert.ToByte(115)));
                Engine.window.Draw(TempShapeObj);
            }
        }
    }
}