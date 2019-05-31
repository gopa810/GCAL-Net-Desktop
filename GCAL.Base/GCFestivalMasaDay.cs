using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base
{
    public class GCFestivalMasaDay: GCFestivalTithiMasa
    {
        public GCFestivalMasaDay()
            : base()
        {
        }

        public override void updateListBoxEntry(GCListBoxEntry lb)
        {
            base.updateListBoxEntry(lb);
            lb.Subtitle = string.Format("{0} Tithi in {1} Masa", GCTithi.GetName(nTithi), GCMasa.GetName(nMasa));
        }

        public override bool IsFestivalDay(GCFestivalSpecialExecutor exec)
        {
            return testFestival(exec, true);
        }
    }
}
