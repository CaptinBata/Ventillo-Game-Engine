using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;
using SFML.System;

using Ventillo.System;
using Ventillo.GameObjects;
using Ventillo.Utils;
using Color = Ventillo.System.Color;

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
                    new Color(209,49,17,155),
                    new Color()
                ),
                new DrawObject(
                    new List<Vector>(){
                        new Vector(playableArea.Min.x, playableArea.Min.y - 1),
                        new Vector(playableArea.Max.x, playableArea.Min.y - 1),
                        new Vector(playableArea.Max.x, playableArea.Min.y + 1),
                        new Vector(playableArea.Min.x, playableArea.Min.y + 1),
                    },
                    new Color(209,49,17,155),
                    new Color()
                ),
                new DrawObject(
                    new List<Vector>(){
                        new Vector(playableArea.Max.x + 1, playableArea.Min.y),
                        new Vector(playableArea.Max.x + 1, playableArea.Max.y),
                        new Vector(playableArea.Max.x - 1, playableArea.Max.y),
                        new Vector(playableArea.Max.x - 1, playableArea.Min.y),
                    },
                    new Color(209,49,17,155),
                    new Color()
                ),
                new DrawObject(
                    new List<Vector>(){
                        new Vector(playableArea.Max.x, playableArea.Max.y - 1),
                        new Vector(playableArea.Max.x, playableArea.Max.y + 1),
                        new Vector(playableArea.Min.x, playableArea.Max.y + 1),
                        new Vector(playableArea.Min.x, playableArea.Max.y - 1),
                        },
                    new Color(209,49,17,155),
                    new Color()
                ),
                new DrawObject(
                    new List<Vector>(){
                        new Vector(xMidPosition - 1, playableArea.Min.y),
                        new Vector(xMidPosition + 1, playableArea.Min.y),
                        new Vector(xMidPosition + 1, playableArea.Max.y),
                        new Vector(xMidPosition - 1, playableArea.Max.y),
                        },
                    new Color(209,49,17,155),
                    new Color()
                ),
                new DrawObject(
                    new List<Vector>(){
                        new Vector(playableArea.Min.x, yMidPosition - 1),
                        new Vector(playableArea.Max.x, yMidPosition - 1),
                        new Vector(playableArea.Max.x, yMidPosition + 1),
                        new Vector(playableArea.Min.x, yMidPosition + 1),
                        },
                    new Color(209,49,17,155),
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
                new Color(209, 49, 17, 155)
            );
        }

        public void DrawObjectBounds(GameObject objectToDraw)
        {
            var min =objectToDraw.MinMax.Min;
            var max = objectToDraw.MinMax.Max;

            ConvexShape TempShape = new ConvexShape();
            TempShape.Position = new Vector2f().UseVentilloVector(objectToDraw.Position); 
            TempShape.SetPointCount(4);

            TempShape.SetPoint(0, new Vector2f().UseVentilloVector(min));
            TempShape.SetPoint(1, new Vector2f().UseVentilloVector(new Vector(max.x, min.y)));
            TempShape.SetPoint(2, new Vector2f().UseVentilloVector(max));
            TempShape.SetPoint(3, new Vector2f().UseVentilloVector(new Vector(min.x, max.y)));

            setDrawModes(TempShape, new Color(), new Color(209, 49, 17, 155));
            Engine.window.Draw(TempShape);

            for (var drawableIndex = 0; drawableIndex < objectToDraw.DrawObjects.Count; drawableIndex++)
            {
                var drawable = objectToDraw.DrawObjects.ElementAt(drawableIndex);

                var minObj= drawable.MinMax.Min;
                var maxObj = drawable.MinMax.Max;

                ConvexShape TempShapeObj = new ConvexShape();
                TempShapeObj.Position = new Vector2f().UseVentilloVector(objectToDraw.Position);
                TempShapeObj.SetPointCount(4);

                TempShapeObj.SetPoint(0, new Vector2f().UseVentilloVector(minObj));
                TempShapeObj.SetPoint(1, new Vector2f().UseVentilloVector(new Vector(maxObj.x, minObj.y)));
                TempShapeObj.SetPoint(2, new Vector2f().UseVentilloVector(maxObj));
                TempShapeObj.SetPoint(3, new Vector2f().UseVentilloVector(new Vector(minObj.x, maxObj.y)));

                setDrawModes(TempShapeObj, new Color(), new Color(102, 225, 0, 155));
                Engine.window.Draw(TempShapeObj);
            }
        }
    }
}