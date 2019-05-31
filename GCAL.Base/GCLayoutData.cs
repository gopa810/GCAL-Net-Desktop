using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base
{
    public class GCLayoutData
    {
        public static int textSizeH1 = 36;
        public static int textSizeH2 = 32;
        public static int textSizeText = 24;
        public static int textSizeNote = 16;

        private static int[][] layoutSizes = new int[][] {
            new int [] { 14, 20, 30, 25 },
            new int [] { 15, 22, 33, 28 },
            new int [] { 16, 24, 36, 32 },
            new int [] { 17, 26, 39, 35 },
            new int [] { 18, 28, 42, 37 },
        };

        private static int layoutIndex = 2;

        public static int LayoutSizeIndex
        {
            get
            {
                return layoutIndex;
            }
            set
            {
                if (value >= 0 && value < 5)
                {
                    textSizeNote = layoutSizes[value][0];
                    textSizeText = layoutSizes[value][1];
                    textSizeH1 = layoutSizes[value][2];
                    textSizeH2 = layoutSizes[value][3];
                    layoutIndex = value;
                }
            }
        }
    }
}
