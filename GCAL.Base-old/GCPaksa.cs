using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base
{
    public class GCPaksa
    {
        public static char GetAbbr(int i)
        {
            string s = GetName(i);
            if (s.Length < 1)
                return ' ';
            return s[0];
        }

        public static string GetName(int i)
        {
            return (i == 1 
                ? GCStrings.Localized("Gaura") 
                : GCStrings.Localized("Krsna"));
        }

    }
}
