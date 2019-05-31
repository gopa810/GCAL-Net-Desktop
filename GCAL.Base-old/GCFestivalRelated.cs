using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base
{
    public class GCFestivalRelated: GCFestivalBase
    {
        public GCFestivalRelated()
            : base()
        {
        }

        public override void updateListBoxEntry(GCListBoxEntry lb)
        {
            base.updateListBoxEntry(lb);
            lb.Subtitle = string.Format("Offset in days: {0}", DayOffset);
        }
    }
}
