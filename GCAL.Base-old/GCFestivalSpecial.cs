using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public class GCFestivalSpecial: GCFestivalBase
    {
        public int nMasaMin = 0;
        public int nMasaMax = 11;

        private GSList script = null;
        private string scriptText = string.Empty;

        public const string InstructionTag = "BeginScript";
        public const string InstructionEndTag = "EndScript";

        public string Script
        {
            get
            {
                return scriptText;
            }
            set
            {
                scriptText = value.TrimEnd();
                script = new GSList();
                script.readList(value);
            }
        }

        public override void updateListBoxEntry(GCListBoxEntry lb)
        {
            base.updateListBoxEntry(lb);
            if (nMasaMax == nMasaMin)
                lb.Subtitle = string.Format("Event active in {0} Masa", GCMasa.GetName(nMasaMin));
            else
                lb.Subtitle = string.Format("Event active from {0} to {1} Masa", GCMasa.GetName(nMasaMin),
                    GCMasa.GetName(nMasaMax));
        }

        protected override string getEncoded()
        {
            string s = base.getEncoded();
            return string.Format("{0}|{1}|{2}", s, nMasaMin, nMasaMax);
        }

        protected override string[] setEncoded(string[] sa)
        {
            sa = base.setEncoded(sa);
            if (sa.Length > 1)
            {
                nMasaMin = StringToInt(sa[0], 0);
                nMasaMax = StringToInt(sa[1], 11);
            }
            return GCFestivalBase.Subarray(sa, 2);
        }

        public override bool IsFestivalDay(GCFestivalSpecialExecutor exec)
        {
            VAISNAVADAY t = exec.day(-DayOffset);
            if (nMasaMin <= nMasaMax)
            {
                if (t.astrodata.Masa < nMasaMin || t.astrodata.Masa > nMasaMax)
                    return false;
            }
            else
            {
                if (t.astrodata.Masa < nMasaMin && t.astrodata.Masa > nMasaMax)
                    return false;
            }

            GSCore c = exec.ExecuteElement(script);
            if (c is GSReturn)
            {
                bool retval = c.getBooleanValue();
                return retval;
            }

            return false;
        }
    }
}
