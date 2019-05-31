using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base.Scripting
{
    public class GDTextStyle
    {
        public GDStyleKey Key { get; set; }
        public object Value { get; set; }

        public GDTextStyle()
        {
        }

        public GDTextStyle(GDStyleKey key, object value)
        {
            Key = key;
            Value = value;
        }
    }

    public enum GDStyleKey
    {
        StyleName,

        TextSize,
        TextAlign,
        FontFamily,
        TextColor,
        BackgroundColor,
        TextStyle,
        TextWidth,

        LeftMarginSize,
        TopMarginSize,
        RightMarginSize,
        BottomMarginSize,

        LeftPaddingSize,
        TopPaddingSize,
        RightPaddingSize,
        BottomPaddingSize,

        LeftBorderSize,
        TopBorderSize,
        RightBorderSize,
        BottomBorderSize,

        LeftBorderColor,
        TopBorderColor,
        RightBorderColor,
        BottomBorderColor,

        VerticalAlign,
    }

    public enum GDLengthUnit
    {
        Emspace,
        Pixel,
        Point,
        Inch,
        Milimeter
    }

    public class GDLength
    {
        public float Value { get; set; }

        public static float EmspaceToPixel = 12;
        public static float InchToPixel = 72;
        public static float PointToPixel = 1;

        public GDLength()
        {
        }

        public GDLength(string txt)
        {
            Text = txt;
        }

        public GDLength(float points)
        {
            Value = points;
        }

        public GDLength(float value, GDLengthUnit unt)
        {
            switch (unt)
            {
                case GDLengthUnit.Emspace: Emspace = value; break;
                case GDLengthUnit.Inch: Inches = value; break;
                case GDLengthUnit.Milimeter: Milimeter = value; break;
                case GDLengthUnit.Pixel: Pixels = value; break;
                case GDLengthUnit.Point: Points = value; break;
            }
        }

        public float Milimeter
        {
            get { return Value * 25.4f * PointToPixel / InchToPixel; }
            set { Value = (value * InchToPixel) / (PointToPixel * 25.4f); }
        }

        public float Inches
        {
            get { return Value * PointToPixel / InchToPixel; }
            set { Value = value * InchToPixel / PointToPixel; }
        }

        public float Points
        {
            get { return Value; }
            set { Value = value; }
        }

        public float Pixels
        {
            get { return Value * PointToPixel; }
            set { Value = value / PointToPixel; }
        }

        public float Emspace
        {
            get { return Value * PointToPixel / EmspaceToPixel; }
            set { Value = value * EmspaceToPixel / PointToPixel; }
        }

        private float NumberExceptLast2(string s)
        {
            float val = 0;
            if (float.TryParse(s.Substring(0, s.Length - 2), out val))
            {
                return val;
            }
            else
            {
                throw new Exception("Invalid length in document: " + s);
            }
        }

        public string Text
        {
            set
            {
                if (value.EndsWith("em"))
                {
                    Emspace = NumberExceptLast2(value);
                }
                else if (value.EndsWith("in"))
                {
                    Inches = NumberExceptLast2(value);
                }
                else if (value.EndsWith("mm"))
                {
                    Milimeter = NumberExceptLast2(value);
                }
                else if (value.EndsWith("pt"))
                {
                    Points = NumberExceptLast2(value);
                }
                else if (value.EndsWith("px"))
                {
                    Pixels = NumberExceptLast2(value);
                }
                else
                {
                    float val = 0;
                    if (float.TryParse(value, out val))
                    {
                        Value = val;
                    }
                    else
                    {
                        throw new Exception("Invalid length in document: " + value);
                    }
                }
            }
        }

        public float GetValue(GDLengthUnit desiredUnit)
        {
            switch (desiredUnit)
            {
                case GDLengthUnit.Emspace:
                    return Emspace;
                case GDLengthUnit.Inch:
                    return Inches;
                case GDLengthUnit.Milimeter:
                    return Milimeter;
                case GDLengthUnit.Pixel:
                    return Pixels;
                case GDLengthUnit.Point:
                    return Points;
            }

            return Value;
        }
    }

    public class GDStyleColor
    {
        /// <summary>
        /// Value 0.0 - 1.0
        /// </summary>
        public float Red { get; set; }

        /// <summary>
        /// Value 0.0 - 1.0
        /// </summary>
        public float Green { get; set; }

        /// <summary>
        /// Value 0.0 - 1.0
        /// </summary>
        public float Blue { get; set; }

        /// <summary>
        /// Value 0.0 - 1.0
        /// </summary>
        public float Alpha { get; set; }

        /// <summary>
        /// Value 0 - 255
        /// </summary>
        public int Red255 { get { return Convert.ToInt32(Red * 255); } set { Red = value / 255.0f; } }

        /// <summary>
        /// Value 0 - 255
        /// </summary>
        public int Green255 { get { return Convert.ToInt32(Green * 255); } set { Green = value / 255.0f; } }

        /// <summary>
        /// Value 0 - 255
        /// </summary>
        public int Blue255 { get { return Convert.ToInt32(Blue * 255); } set { Blue = value / 255.0f; } }

        /// <summary>
        /// Value 0 - 255
        /// </summary>
        public int Alpha255 { get { return Convert.ToInt32(Alpha * 255); } set { Alpha = value / 255.0f; } }

        public GDStyleColor(float r, float g, float b)
        {
            Red = r;
            Green = g;
            Blue = b;
            Alpha = 1.0f;
        }

        public GDStyleColor(float r, float g, float b, float a)
        {
            Red = r;
            Green = g;
            Blue = b;
            Alpha = a;
        }

        public override int GetHashCode()
        {
            return Convert.ToInt32(Red*255) + Convert.ToInt32(Green*255)<<8 + Convert.ToInt32(Blue*255)<<16 + Convert.ToInt32(Alpha*128)<<24;
        }

        public override bool Equals(object obj)
        {
            if (obj is GDStyleColor)
            {
                GDStyleColor clr = obj as GDStyleColor;
                if (clr.Alpha != Alpha)
                    return false;
                if (clr.Red != Red)
                    return false;
                if (clr.Green != Green)
                    return false;
                if (clr.Blue != Blue)
                    return false;
                return true;
            }

            return base.Equals(obj);
        }
    }
}
