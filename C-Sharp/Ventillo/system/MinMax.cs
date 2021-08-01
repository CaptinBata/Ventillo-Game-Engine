using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventillo.System;

namespace Ventillo.System
{
    public class MinMax
    {
        public Vector Min;
        public Vector Max;

        public MinMax()
        {

        }

        public MinMax(Vector Min, Vector Max)
        {
            this.Min = Min;
            this.Max = Max;
        }

        public bool PointIntersects(Vector point)
        {
            if (point.x > this.Min.x && point.y > this.Min.y &&
                point.x < this.Max.x && point.y < this.Max.y)
                return true;
            return false;
        }
    }
}