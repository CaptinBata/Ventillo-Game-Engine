using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;

namespace Ventillo.System
{
    public class Color
    {
        SFML.Graphics.Color sfmlColor;

        public Color() { }
        public Color(int red, int green, int blue, int alpha)
        {
            sfmlColor = new SFML.Graphics.Color(Convert.ToByte(red), Convert.ToByte(green), Convert.ToByte(blue), Convert.ToByte(alpha));
        }

        internal SFML.Graphics.Color GetSFMLColor()
        {
            return sfmlColor;
        }
    }
}
