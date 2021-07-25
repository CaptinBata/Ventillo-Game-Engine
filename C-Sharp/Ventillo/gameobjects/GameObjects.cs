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
        public List<DrawObject> DrawObjects = new List<DrawObject>();
        public Vector Position = null;
        public bool ToDelete = false;
        public MinMax MinMax;

        protected Font font = null;

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
            for (var drawObjectIndex = 0; drawObjectIndex < this.DrawObjects.Count; drawObjectIndex++)
            {
                var drawObject = this.DrawObjects.ElementAt(drawObjectIndex);

                Vector Min = new Vector(double.MaxValue, double.MaxValue);
                Vector Max = new Vector(double.MinValue, double.MinValue);

                for (var pointIndex = 0; pointIndex < drawObject.DrawPoints.Count; pointIndex++)
                {
                    var point = drawObject.DrawPoints.ElementAt(pointIndex);

                    if (point.x < Min.x)
                        Min = new Vector(point.x, Min.y);
                    if (point.y < Min.y)
                        Min = new Vector(Min.x, point.y);


                    if (point.x > Max.x)
                        Max = new Vector(point.x, Max.y);
                    if (point.y > Max.y)
                        Max = new Vector(Max.x, point.y);
                };

                MinMax MinMax = new MinMax(Min, Max);

                var index = this.DrawObjects.IndexOf(drawObject);
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

            for (var drawObjectIndex = 0; drawObjectIndex < this.DrawObjects.Count; drawObjectIndex++)
            {
                var drawObject = this.DrawObjects.ElementAt(drawObjectIndex);

                if (drawObject.MinMax.Max.x > Max.x)
                    Max = new Vector(drawObject.MinMax.Max.x, Max.y);
                if (drawObject.MinMax.Max.y > Max.y)
                    Max = new Vector(Max.x, drawObject.MinMax.Max.y);
                if (drawObject.MinMax.Min.x < Min.x)
                    Min = new Vector(drawObject.MinMax.Min.x, Min.y);
                if (drawObject.MinMax.Min.y < Min.y)
                    Min = new Vector(Min.x, drawObject.MinMax.Min.y);
            }

            MinMax MinMax = new MinMax(Min, Max);

            this.MinMax = MinMax; //Assign overall box of the entire object
        }

        private void GetObjectBounds() //Used to find the AABB (Axis-Aligned Bounding Box). Basically the basic box around the object to be used as primitive hit detection
        {
            this.AssignIndividualObjectBounds();
            this.AssignTotalObjectBounds();
        }

        public virtual void Draw()
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
            TempShape.SetPointCount(4);

            TempShape.SetPoint(0, new Vector2f(-0.5f, 0.5f));
            TempShape.SetPoint(1, new Vector2f(0.5f, 0.5f));
            TempShape.SetPoint(2, new Vector2f(0.5f, -0.5f));
            TempShape.SetPoint(3, new Vector2f(-0.5f, -0.5f));

            Engine.window.Draw(TempShape);
        }

        protected void DrawByLine(List<DrawObject> Drawobjects)
        {
            for (var drawableIndex = 0; drawableIndex < Drawobjects.Count; drawableIndex++)
            {
                var drawable = Drawobjects.ElementAt(drawableIndex);

                ConvexShape TempShape = new ConvexShape();
                TempShape.Position = new Vector2f().UseVentilloVector(this.Position);

                uint index = 0;
                TempShape.SetPointCount((uint)(drawable.DrawPoints.Count));

                for (var drawPointIndex = 0; drawPointIndex < drawable.DrawPoints.Count; drawPointIndex++)
                {
                    var drawPoint = drawable.DrawPoints.ElementAt(drawPointIndex);

                    TempShape.SetPoint(index, new Vector2f().UseVentilloVector(drawPoint));
                    index++;
                }

                this.setDrawModes(TempShape, drawable.StrokeColour, drawable.FillColour);
                Engine.window.Draw(TempShape);
            }
        }

        public Font LoadFont(string fontPath)
        {
            Font font = null;
            try
            {
                return font = new Font(fontPath);
            }
            catch (Exception error)
            {
                throw new Exception($"Failed to load font ${font}. Exception: {error}");
            }
        }

        private Font LoadDebugText()
        {
            return new Font("../fonts/Gill Sans MT.tff");
        }

        protected void DrawByText(string text, Vector position, Color colour, uint fontSize = 14)
        {
            if (this.font != null)
                this.font = this.LoadDebugText();

            Text SFMLtext = new Text(text, this.font, fontSize);

            // set the color
            SFMLtext.FillColor = colour;

            // set the text style
            SFMLtext.Style = Text.Styles.Regular;

            Engine.window.Draw(SFMLtext);
        }

        protected void DrawByPixel(List<DrawObject> Drawobjects)
        {
            for (var drawableIndex = 0; drawableIndex < Drawobjects.Count; drawableIndex++)
            {
                var drawable = Drawobjects.ElementAt(drawableIndex);

                for (var drawPointIndex = 0; drawPointIndex < drawable.DrawPoints.Count; drawPointIndex++)
                {
                    var drawPoint = drawable.DrawPoints.ElementAt(drawPointIndex);

                    ConvexShape TempShape = new ConvexShape();
                    TempShape.Position = new Vector2f().UseVentilloVector(this.Position);
                    TempShape.SetPointCount(4);

                    TempShape.SetPoint(0, new Vector2f((float)(drawPoint.x) - 0.5f, (float)(drawPoint.x) + 0.5f));
                    TempShape.SetPoint(1, new Vector2f((float)(drawPoint.x) + 0.5f, (float)(drawPoint.x) + 0.5f));
                    TempShape.SetPoint(2, new Vector2f((float)(drawPoint.x) + 0.5f, (float)(drawPoint.x) - 0.5f));
                    TempShape.SetPoint(3, new Vector2f((float)(drawPoint.x) - 0.5f, (float)(drawPoint.x) - 0.5f));

                    this.setDrawModes(TempShape, drawable.StrokeColour, drawable.FillColour);
                    Engine.window.Draw(TempShape);
                }
            }
        }

        protected void DrawByCircle(List<DrawObject> DrawObjects)
        {
            for (var drawableIndex = 0; drawableIndex < this.DrawObjects.Count; drawableIndex++)
            {
                var drawable = this.DrawObjects.ElementAt(drawableIndex);

                for (var drawPointIndex = 0; drawableIndex < drawable.DrawPoints.Count; drawPointIndex++)
                {
                    var drawPoint = drawable.DrawPoints.ElementAt(drawPointIndex);

                    CircleShape TempShape = new CircleShape((float)(drawPoint.x));
                    TempShape.Position = new Vector2f().UseVentilloVector(this.Position);

                    this.setDrawModes(TempShape, drawable.StrokeColour, drawable.FillColour);
                    Engine.window.Draw(TempShape);
                }
            }
        }

        protected void setDrawModes(Shape TempShape, Color? StrokeStyle, Color? FillStyle)
        {
            if (StrokeStyle != null)
            {
                TempShape.OutlineColor = (Color)StrokeStyle;
            }

            if (FillStyle != null)
            {
                TempShape.FillColor = (Color)FillStyle;
            }
        }
    }

}
