using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using GCAL.Base;


namespace GCAL.MapCalculator
{
    public class QuandrantResult
    {
        public static QuandrantResult Empty { get; set; }
        static QuandrantResult()
        {
            Empty = new QuandrantResult();
        }

        public int ResultId = -1;
        public Color color = Color.Transparent;
        public Color colorFull = Color.Transparent;
        public VAISNAVADAY day = null;

        public override string ToString()
        {
            string vrata = day.ekadasi_vrata_name;
            if (day.nMahadvadasiID != MahadvadasiType.EV_NULL && day.nMahadvadasiID != MahadvadasiType.EV_SUDDHA)
                vrata = GCEkadasi.GetMahadvadasiName(day.nMahadvadasiID);
            return day.date.ToString() + ", " + vrata;
        }
    }
}
