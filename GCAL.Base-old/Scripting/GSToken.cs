using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base.Scripting
{
    public class GSToken: GSCore
    {
        public string Token = String.Empty;

        public GSToken()
        {
        }

        public GSToken(string s)
        {
            Token = s;
        }

        public override string getStringValue()
        {
            return Token;
        }

        public override string ToString()
        {
            return Token;
        }

        public override void writeScript(int level, System.IO.StreamWriter sw)
        {
            sw.Write(Token);
        }
    }
}
