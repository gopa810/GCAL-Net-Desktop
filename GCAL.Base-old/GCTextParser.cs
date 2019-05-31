using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base
{
    public class GCTextParser
    {
        private string[] lines = null;
        private int currLine = 0;

        public void SetTarget(string s)
        {
            lines = s.Replace("\r\n", "\n").Split('\n');
            currLine = 0;
        }

        public string GetNextLine()
        {
            if (currLine >= lines.Length)
                return null;
            currLine++;
            return lines[currLine - 1];
        }
    }
}
