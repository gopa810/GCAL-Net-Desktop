using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.CalendarDataView
{
    public struct CDVBorder
    {
        public int Top;
        public int Left;
        public int Right;
        public int Bottom;

        public CDVBorder(int value)
        {
            Top = Bottom = Left = Right = value;
        }

        public int All
        {
            set
            {
                Left = Right = Top = Bottom = value;
            }
        }
        public CDVBorder(int leftRight, int topBottom)
        {
            Left = Right = leftRight;
            Top = Bottom = topBottom;
        }

        public int GetMax()
        {
            return Math.Max(Left, Math.Max(Right, Math.Max(Top, Bottom)));
        }
    }
}
