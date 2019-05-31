using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base
{
    public class GaurabdaDate
    {
        public int tithi;
        public int masa;
        public int gyear;

        public GaurabdaDate()
        {
        }

        public GaurabdaDate(int t, int m, int y)
        {
            tithi = t;
            masa = m;
            gyear = y;
        }

        public void next()
        {
            tithi++;
            if (tithi >= 30)
            {
                tithi %= 30;
                masa++;
            }
            if (masa >= 12)
            {
                masa %= 12;
                gyear++;
            }
        }

        public void prev()
        {
            if (tithi == 0)
            {
                if (masa == 0)
                {
                    masa = 11;
                    tithi = 29;
                }
                else
                {
                    masa--;
                    tithi = 29;
                }
            }
            else
            {
                tithi--;
            }
        }

        public void Set(GaurabdaDate va)
        {
            tithi = va.tithi;
            masa = va.masa;
            gyear = va.gyear;
        }

    }
}
