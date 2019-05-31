using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GCAL.CalendarDataView
{
    public struct CDVColor
    {
        public static readonly CDVColor Transparent = new CDVColor(Color.Transparent.A, Color.Transparent.R, Color.Transparent.G, Color.Transparent.B);
        public static readonly CDVColor Black = new CDVColor(Color.Black.A, 0, 0, 0);
        public static readonly CDVColor LightYellow = new CDVColor(Color.LightYellow.A, Color.LightYellow.R, Color.LightYellow.G, Color.LightYellow.B);
        public static readonly CDVColor White = new CDVColor(Color.White.A, Color.White.R, Color.White.G, Color.White.B);
        public static readonly CDVColor Red = new CDVColor(Color.Red.A, Color.Red.R, Color.Red.G, Color.Red.B);
        public static readonly CDVColor Blue = new CDVColor(Color.Blue.A, Color.Blue.R, Color.Blue.G, Color.Blue.B);
        public static readonly CDVColor Green = new CDVColor(Color.Green.A, Color.Green.R, Color.Green.G, Color.Green.B);

        public byte A;
        public byte R;
        public byte G;
        public byte B;

        public CDVColor(byte alpha, byte red, byte green, byte blue)
        {
            A = alpha;
            R = red;
            G = green;
            B = blue;
        }

        public override bool Equals(object obj)
        {
            if (obj is CDVColor)
            {
                CDVColor c = (CDVColor)obj;
                return (A == c.A && R == c.R && G == c.G && B == c.B);
            }
            else
                return base.Equals(obj);
        }
    }
}
