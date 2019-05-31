using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GCAL.CalendarDataView
{
    public class CDVContext
    {
        public Graphics g;
        public bool rulersReflow = false;
        public bool rulersChanged = false;
        public Rectangle ScreenRect = Rectangle.Empty;

        public static Dictionary<CDVColor, Brush> Brushes = new Dictionary<CDVColor, Brush>();
        public static Dictionary<string,Pen> Pens = new Dictionary<string, Pen>();
        public static Dictionary<string, Font> Fonts = new Dictionary<string, Font>();

        public static StringFormat StringFormatCenterCenter;

        static CDVContext()
        {
            StringFormatCenterCenter = new StringFormat();
            StringFormatCenterCenter.Alignment = StringAlignment.Center;
            StringFormatCenterCenter.LineAlignment = StringAlignment.Center;
        }

        public static Brush GetBrush(CDVColor color)
        {
            if (Brushes.ContainsKey(color))
                return Brushes[color];

            Color c = Color.FromArgb(color.A, color.R, color.G, color.B);
            Brushes[color] = new SolidBrush(c);
            return Brushes[color];
        }

        public static Pen GetPen(int width, CDVColor color)
        {
            if (width < 1) width = 1;
            if (width > 100) width = 100;
            string key = string.Format("{0}/{1}/{2}/{3}/{4}", width, color.A, color.R, color.G, color.B);
            if (Pens.ContainsKey(key))
                return Pens[key];

            Color c = Color.FromArgb(color.A, color.R, color.G, color.B);
            Pens[key] = new Pen(c, width / 10f);
            return Pens[key];
        }

        public static Font GetFont(string familyName, int size, bool bold, bool italic, bool underline)
        {
            if (string.IsNullOrWhiteSpace(familyName))
                familyName = "Times";

            string key = string.Format("{0}/{1}/{2}{3}{4}", familyName, size, bold ? 1 : 0, italic ? 1 : 0, underline ? 1 : 0);
            if (Fonts.ContainsKey(key))
                return Fonts[key];

            Fonts[key] = new Font(new FontFamily(familyName), (float)size,
                (bold ? FontStyle.Bold : FontStyle.Regular)
                | (italic ? FontStyle.Italic : FontStyle.Regular)
                | (underline ? FontStyle.Underline : FontStyle.Regular));

            return Fonts[key];
        }
    }
}
