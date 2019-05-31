using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.CalendarDataView
{
    public class CDVParaStyle
    {
        public string StyleName = "";

        private static CDVParaStyle ps_empty;
        public static CDVParaStyle Empty
        {
            get
            {
                return ps_empty;
            }
        }

        static CDVParaStyle()
        {
            ps_empty = new CDVParaStyle();
        }

        public CDVAlign Align = CDVAlign.Left;
        public CDVBorder Padding = new CDVBorder(0);
        public CDVBorder Margin = new CDVBorder(0);
        public int RoundCorner = 0;
        public CDVBorder BorderWidth = new CDVBorder(0);
        public CDVColor BorderColor = CDVColor.Transparent;
        public CDVColor BackgroundColor = CDVColor.Transparent;
    }
}
