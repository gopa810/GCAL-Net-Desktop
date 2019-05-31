using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public class GCFestivalEkadasi: GCFestivalBase
    {
        public int nMasa { get; set; }
        public int nPaksa { get; set; }

        public GCFestivalEkadasi()
            : base()
        {
            nMasa = 0;
            nPaksa = 0;
        }

        public override void updateListBoxEntry(GCListBoxEntry lb)
        {
            base.updateListBoxEntry(lb);
            lb.Subtitle = string.Format("Ekadasi in {0} Masa, {1} Paksa", GCMasa.GetName(nMasa), GCPaksa.GetName(nPaksa));
        }

        protected override string getEncoded()
        {
            return string.Format("{0}|{1}|{2}", base.getEncoded(), nMasa, nPaksa);
        }

        protected override string[] setEncoded(string[] sa)
        {
            sa = base.setEncoded(sa);
            if (sa.Length > 1)
            {
                nMasa = StringToInt(sa[0], 0);
                nPaksa = StringToInt(sa[1], 0);
            }
            return GCFestivalBase.Subarray(sa, 2);
        }

        public override Scripting.GSList getScript()
        {
            GSList list = base.getScript();
            list.Add(new GSList(new GSToken("condition"), new GSToken("(x (x calendar getDay (+ currIndex " + (-DayOffset).ToString() + ")) isEkadasiFast " + nPaksa + " " + nMasa + ")")));
            return list;
        }

        public override bool IsFestivalDay(GCFestivalSpecialExecutor exec)
        {
            VAISNAVADAY testDay = exec.day(-DayOffset);

            return (testDay.astrodata.Masa == nMasa
                && testDay.astrodata.sunRise.Paksa == nPaksa
                && (GCTithi.TITHI_EKADASI(testDay.astrodata.sunRise.Tithi)
                || GCTithi.TITHI_DVADASI(testDay.astrodata.sunRise.Tithi))
                && testDay.nFastID == FastType.FAST_EKADASI);
        }
    }
}
