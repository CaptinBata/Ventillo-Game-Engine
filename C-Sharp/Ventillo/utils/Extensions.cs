using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        internal static FieldInfo[] GetFields(object obj)
        {
            var type = obj.GetType();
            var props = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            return props;
        }

        public static string ToObjectString(this object obj)
        {
            if (obj != null)
            {
                string objString = "\n{\n";

                var type = obj.GetType();
                var props = GetFields(obj);

                objString += $"    Type: {type}\n";

                foreach (var prop in props)
                {
                    var propValue = prop.GetValue(obj);
                    objString += $"    {prop}: {propValue}\n";
                }
                objString += "}\n";

                return objString;
            }
            return "";
        }
    }
}