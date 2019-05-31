using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base.Scripting
{
    public class GDTextRun: GDNode
    {
        public string Text { get; set; }

        public GDTextRun()
        {
        }

        public GDTextRun(string s)
        {
            Text = s;
        }

        public GDTextRun(string format, params object[] args)
        {
            Text = string.Format(format, args);
        }
    }
}
