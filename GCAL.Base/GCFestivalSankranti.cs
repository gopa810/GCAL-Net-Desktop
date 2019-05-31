using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public class GCFestivalSankranti: GCFestivalBase
    {
        public int RasiOfSun { get; set; }

        public GCFestivalSankranti()
        {
            RasiOfSun = SankrantiId.MAKARA_SANKRANTI;
        }

        public override void updateListBoxEntry(GCListBoxEntry lb)
        {
            base.updateListBoxEntry(lb);
            lb.Subtitle = string.Format("{0} Sankranti, {1} days offset", GCRasi.GetName(RasiOfSun), DayOffset);
        }

        protected override string getEncoded()
        {
            return string.Format("{0}|{1}", base.getEncoded(), RasiOfSun);
        }

        protected override string[] setEncoded(string[] sa)
        {
            sa = base.setEncoded(sa);
            if (sa.Length > 0)
            {
                RasiOfSun = StringToInt(sa[0], 0);
            }
            return GCFestivalBase.Subarray(sa, 1);
        }

        public override Scripting.GSList getScript()
        {
            GSList list = base.getScript();
            list.Add(new GSList(new GSToken("condition"), new GSToken("(x (x calendar getDay (+ currIndex " + (-DayOffset).ToString() + ")) hasSankranti " + RasiOfSun + ")")));
            return list;
        }

        public override bool IsFestivalDay(GCFestivalSpecialExecutor exec)
        {
            return (exec.day(-DayOffset).sankranti_zodiac == RasiOfSun);
        }
    }
}
