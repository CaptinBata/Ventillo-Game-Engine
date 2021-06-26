using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;

namespace Ventillo.GameObjects
{
    public struct DrawObject
    {
        public List<Vector> DrawPoints;
        public string FillColour;
        public string StrokeColour;
        public MinMax? MinMax;
        public string? text;
    }
    public class GameObject
    {
        List<DrawObject> DrawObjects = new List<DrawObject>();
        Vector Position = null;
        bool ToDelete = false;
        MinMax MinMax;
        List<ConvexShape> SFMLShapes = new List<ConvexShape>();

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

        private void SetDrawObject(List<DrawObject> DrawObjects)
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
            this.DrawByLine();
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
    }

}
