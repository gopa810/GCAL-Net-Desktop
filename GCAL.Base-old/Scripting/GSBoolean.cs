using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base.Scripting
{
    public class GSBoolean: GSCore
    {
        public bool Value = false;

        public GSBoolean()
        {
        }

        public GSBoolean(bool b)
        {
            Value = b;
        }

        public override string getStringValue()
        {
            return Value ? "1" : "0";
        }

        public override long getIntegerValue()
        {
            return Value ? 1 : 0;
        }

        public override bool getBooleanValue()
        {
            return Value;
        }

        public override double getDoubleValue()
        {
            return Value ? 1.0 : 0.0;
        }

        public override void writeScript(int level, System.IO.StreamWriter sw)
        {
            sw.Write(getIntegerValue().ToString());
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
