using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventillo.Utils;

namespace Ventillo.GameObjects
{
    public class MinMax
    {
        public  Vector Min;
        public Vector Max;

        public MinMax(Vector Min, Vector Max)
        {
            this.Min = Min;
            this.Max = Max;
        }

        bool pointIntersects(Vector point)
        {
            if (point.x > this.Min.x && point.y > this.Min.y &&
                point.x < this.Max.x && point.y < this.Max.y)
                return true;
            return false;
        }
    }
}