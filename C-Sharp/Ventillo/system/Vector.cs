using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventillo.System
{
    public class Vector
    {
        public double x;
        public double y;

        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector Translate(Vector vector_1, Vector vector_2)
        {
            return new Vector(
                vector_1.x + vector_2.x,
                vector_1.y + vector_2.y
            );
        }

        public void Translate(Vector translateVector)
        {
            x += translateVector.x;
            y += translateVector.y;
        }

        public static Vector RotateVectorAroundPoint(Vector vector, Vector point, double angle)
        {
            double radians = angle * (Math.PI / 180.0);

            double sine = Math.Sin(radians);
            double cosine = Math.Cos(radians);

            Vector temp = new Vector(
                vector.x - point.x,
                vector.y - point.y
            );


            double newX = temp.x * cosine - temp.y * sine;
            double newY = temp.x * sine + temp.y * cosine;

            temp.x = newX + point.x;
            temp.y = newY + point.y;

            return temp;
        }
    }
}