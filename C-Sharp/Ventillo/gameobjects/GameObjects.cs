using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;
using SFML.System;

using Ventillo;
using Ventillo.Utils;

namespace Ventillo.GameObjects
{
    public struct DrawObject
    {
        public List<Vector> DrawPoints;
        public Color FillColour;
        public Color StrokeColour;
        public MinMax? MinMax;
        public string? Text;

        public DrawObject(List<Vector> drawPoints, Color fillColor, Color strokeColour, MinMax minMax = null, string text = "")
        {
            DrawPoints = drawPoints;
            FillColour = fillColor;
            StrokeColour = strokeColour;
            MinMax = minMax;
            Text = text;
        }
    }
    public class GameObject
    {
        protected List<DrawObject> DrawObjects = new List<DrawObject>();
        protected Vector Position = null;
        protected bool ToDelete = false;
        protected MinMax MinMax;

        public GameObject(Vector Position)
        {
            this.Position = Position;
        }

        ~GameObject()
        {
            this.DrawObjects = null;
            this.Position = null;
            this.MinMax = null;
        }

        protected void SetDrawObject(List<DrawObject> DrawObjects)
        {
            this.DrawObjects = DrawObjects;
            this.GetObjectBounds();
        }

        private MinMax GetMinMax()
        {
            return this.MinMax;
        }

        public virtual void CheckDelete(List<GameObject> GameObjects)
        {

        }

        public virtual void Update(List<GameObject> GameObjects)
        {

        }

        private bool DetectAABBCollision(GameObject other)
        {
            var collisions = (MinX: false, MaxX: false, MinY: false, MaxY: false);

            Vector OtherMinGlobal = other.ToGlobalCoords(other.GetMinMax().Min);
            Vector OtherMaxGlobal = other.ToGlobalCoords(other.GetMinMax().Max);

            Vector ThisMinGlobal = this.ToGlobalCoords(this.GetMinMax().Min);
            Vector ThisMaxGlobal = this.ToGlobalCoords(this.GetMinMax().Max);

            if (OtherMinGlobal.x > ThisMinGlobal.x && OtherMinGlobal.x < ThisMaxGlobal.x)
                collisions.MinX = true;
            if (OtherMaxGlobal.x > ThisMinGlobal.x && OtherMaxGlobal.x < ThisMaxGlobal.x)
                collisions.MaxX = true;


            if (OtherMinGlobal.y > ThisMinGlobal.y && OtherMinGlobal.y < ThisMaxGlobal.y)
                collisions.MinY = true;
            if (OtherMaxGlobal.y > ThisMinGlobal.y && OtherMaxGlobal.y < ThisMaxGlobal.y)
                collisions.MaxY = true;

            return (collisions.MinX || collisions.MaxX) && (collisions.MinY || collisions.MaxY); //If horizontal point and vertical point overlapping, doesn't matter which ones or if multiple of either
        }

        private void AssignIndividualObjectBounds()
        {
            foreach (DrawObject DrawObject in this.DrawObjects)
            {
                Vector Min = new Vector(double.MaxValue, double.MaxValue);
                Vector Max = new Vector(double.MinValue, double.MinValue);

                foreach (Vector Point in DrawObject.DrawPoints)
                {
                    if (Point.x < Min.x)
                        Min = new Vector(Point.x, Min.y);
                    if (Point.y < Min.y)
                        Min = new Vector(Min.x, Point.y);


                    if (Point.x > Max.x)
                        Max = new Vector(Point.x, Max.y);
                    if (Point.y > Max.y)
                        Max = new Vector(Max.x, Point.y);
                };

                MinMax MinMax = new MinMax(Min, Max);

                var index = this.DrawObjects.IndexOf(DrawObject);
                var OldMinMax = this.DrawObjects.ElementAt(index);
                var NewMinMax = OldMinMax;

                NewMinMax.MinMax = MinMax;
                this.DrawObjects[index] = NewMinMax; //Assign each individual part of a drawObject's minMax
            }
        }

        private void RotateAroundPoint(Vector point, double angle)
        {
            this.Position = Vector.rotateVectorAroundPoint(this.Position, point, angle);
        }

        private void TranslatePosition(Vector otherVector)
        {
            this.Position = Vector.translate(this.Position, otherVector);
        }

        private void AssignTotalObjectBounds()
        {
            Vector Min = new Vector(double.MaxValue, double.MaxValue);
            Vector Max = new Vector(double.MinValue, double.MinValue);

            foreach (DrawObject DrawObject in this.DrawObjects)
            {
                if (DrawObject.MinMax.Max.x > Max.x)
                    Max = new Vector(DrawObject.MinMax.Max.x, Max.y);
                if (DrawObject.MinMax.Max.y > Max.y)
                    Max = new Vector(Max.x, DrawObject.MinMax.Max.y);
                if (DrawObject.MinMax.Min.x < Min.x)
                    Min = new Vector(DrawObject.MinMax.Min.x, Min.y);
                if (DrawObject.MinMax.Min.y < Min.y)
                    Min = new Vector(Min.x, DrawObject.MinMax.Min.y);
            }

            MinMax MinMax = new MinMax(Min, Max);

            this.MinMax = MinMax; //Assign overall box of the entire object
        }

        private void GetObjectBounds() //Used to find the AABB (Axis-Aligned Bounding Box). Basically the basic box around the object to be used as primitive hit detection
        {
            this.AssignIndividualObjectBounds();
            this.AssignTotalObjectBounds();
        }

        public void draw()
        {
            this.DrawByLine(this.DrawObjects);
        }

        public Vector ToGlobalCoords(Vector localVector)
        {
            return new Vector(this.Position.x + localVector.x, this.Position.y + localVector.y);
        }

        public Vector ToLocalCoords(Vector globalVector)
        {
            return new Vector(globalVector.x - this.Position.x, globalVector.y - this.Position.y);
        }

        public double GetWidth()
        {
            return this.MinMax.Max.x - this.MinMax.Min.x;
        }

        public double GetHeight()
        {
            return this.MinMax.Max.y - this.MinMax.Min.y;
        }

        protected void DrawAPixel(Vector Position)
        {
            ConvexShape TempShape = new ConvexShape();
            TempShape.Position = new Vector2f().UseVentilloVector(Position);

            TempShape.SetPoint(0, new Vector2f(-0.5f, 0.5f));
            TempShape.SetPoint(1, new Vector2f(0.5f, 0.5f));
            TempShape.SetPoint(2, new Vector2f(0.5f, -0.5f));
            TempShape.SetPoint(3, new Vector2f(-0.5f, -0.5f));

            Engine.Window.Draw(TempShape);
        }

        protected void DrawByLine(List<DrawObject> Drawobjects)
        {
            foreach (DrawObject Drawable in DrawObjects)
            {
                ConvexShape TempShape = new ConvexShape();

                uint index = 0;
                foreach (Vector DrawPoint in Drawable.DrawPoints)
                {
                    TempShape.SetPoint(index, new Vector2f().UseVentilloVector(this.ToGlobalCoords(DrawPoint)));
                    index++;
                }

                this.setDrawModes(TempShape, Drawable.StrokeColour, Drawable.FillColour);
                Engine.Window.Draw(TempShape);
            }
        }

        protected void DrawByText(List<DrawObject> Drawobjects)
        {

        }

        protected void DrawByPixel(List<DrawObject> Drawobjects)
        {
            foreach (DrawObject Drawable in Drawobjects)
            {
                foreach (Vector DrawPoint in Drawable.DrawPoints)
                {
                    ConvexShape TempShape = new ConvexShape();
                    TempShape.Position = new Vector2f().UseVentilloVector(DrawPoint);

                    TempShape.SetPoint(0, new Vector2f((float)(DrawPoint.x) - 0.5f, (float)(DrawPoint.x) + 0.5f));
                    TempShape.SetPoint(1, new Vector2f((float)(DrawPoint.x) + 0.5f, (float)(DrawPoint.x) + 0.5f));
                    TempShape.SetPoint(2, new Vector2f((float)(DrawPoint.x) + 0.5f, (float)(DrawPoint.x) - 0.5f));
                    TempShape.SetPoint(3, new Vector2f((float)(DrawPoint.x) - 0.5f, (float)(DrawPoint.x) - 0.5f));

                    this.setDrawModes(TempShape, Drawable.StrokeColour, Drawable.FillColour);
                    Engine.Window.Draw(TempShape);
                }
            }
        }

        protected void setDrawModes(ConvexShape TempShape, Color StrokeStyle, Color FillStyle)
        {
            if (StrokeStyle != null)
            {
                TempShape.OutlineColor = StrokeStyle;
            }

            if (FillStyle != null)
            {
                TempShape.FillColor = FillStyle;
            }
        }
    }

}
