using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GCAL.Base
{
    public class GCLog
    {
        public static void Write(string format, params object[] args)
        {
            Debugger.Log(0, "GCLog", string.Format(format, args) + "\n");
        }
    }
}
