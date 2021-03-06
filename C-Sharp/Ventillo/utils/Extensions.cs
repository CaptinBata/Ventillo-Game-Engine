using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;

using Ventillo.System;

namespace Ventillo.Utils
{
    public static class Extensions
    {
        public static Vector2f UseVentilloVector(this Vector2f NewVector2f, Vector Position)
        {
            return new Vector2f((float)(Position.x), (float)(Position.y));
        }

        public static double GetTimeInMilliseconds(this DateTime newDateTime)
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}