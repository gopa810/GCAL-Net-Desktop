using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public class GCFestivalTithiMasa: GCFestivalBase
    {
        public int nTithi;
        public int nMasa;

        public GCFestivalTithiMasa(): base()
        {
            nTithi = 0;
            nMasa = 0;
        }

        public override void updateListBoxEntry(GCListBoxEntry lb)
        {
            base.updateListBoxEntry(lb);
            lb.Subtitle = string.Format("{0} Tithi, {1} Masa", GCTithi.GetName(nTithi), GCMasa.GetName(nMasa));
        }

        protected override string getEncoded()
        {
            return string.Format("{0}|{1}|{2}", base.getEncoded(), nTithi, nMasa);
        }

        protected override string[] setEncoded(string[] sa)
        {
            sa = base.setEncoded(sa);
            if (sa.Length > 1)
            {
                nTithi = StringToInt(sa[0], 0);
                nMasa = StringToInt(sa[1], 0);
            }
            return GCFestivalBase.Subarray(sa, 2);
        }

        public override Scripting.GSList getScript()
        {
            GSList list = base.getScript();
            list.Add(new GSList(new GSToken("condition"), new GSToken("(x (x calendar getDay (+ currIndex " + (-DayOffset).ToString() + ")) isFestivalTithi " + nTithi + " " + nMasa + ")")));
            return list;
        }

        public override bool IsFestivalDay(GCFestivalSpecialExecutor exec)
        {
            return testFestival(exec, false);
        }

        // test if today is tithi of given masa, or day after tithi of given masa
        // while in second cas yesterday is tithi before given tithi and masa
        public bool testFestival(GCFestivalSpecialExecutor exec, bool stickToMasa)
        {
            VAISNAVADAY yesterday = exec.day(-DayOffset - 1);
            VAISNAVADAY today = exec.day(-DayOffset);
            VAISNAVADAY tomorrow = exec.day(-DayOffset + 1);

            if (today.astrodata.Masa == nMasa && today.astrodata.sunRise.Tithi == nTithi)
            {
                if (yesterday.astrodata.Masa == nMasa && yesterday.astrodata.sunRise.Tithi == nTithi)
                    return false;
                else
                    return true;
            }

            if (stickToMasa || GCMasa.IS_EXTRA(today.astrodata.Masa))
            {
                if (today.ksayaMasa == nMasa && today.ksayaTithi == nTithi)
                {
                    return true;
                }
            }
            else
            {
                if (yesterday.ksayaMasa == nMasa && yesterday.ksayaTithi == nTithi)
                {
                    return true;
                }
            }

            return false;
        }


    }
}
