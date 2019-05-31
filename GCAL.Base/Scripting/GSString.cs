using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base.Scripting
{
    public class GSString: GSCore
    {
        private static GSString emptyValue = new GSString();

        public string Value = String.Empty;

        public GSString()
        {
        }

        public GSString(string s)
        {
            Value = s;
        }

        public static GSString Empty
        {
            get
            {
                return emptyValue;
            }
        }

        public override string getStringValue()
        {
            return Value;
        }

        public override long getIntegerValue()
        {
            long l = 0;
            if (long.TryParse(Value, out l))
                return l;
            return 0;
        }

        public override bool getBooleanValue()
        {
            return getIntegerValue() != 0;
        }

        public override double getDoubleValue()
        {
            double d = 0.0;
            if (double.TryParse(Value, out d))
                return d;
            return 0.0;
        }

        public override GSCore GetPropertyValue(string s)
        {
            if (s.Equals("length"))
            {
                return new GSNumber() { IntegerValue = Value.Length };
            }
            if (s.Equals("lower"))
            {
                GSString str = new GSString();
                str.Value = Value.ToLower();
                return str;
            }
            if (s.Equals("upper"))
            {
                GSString str = new GSString();
                str.Value = Value.ToUpper();
                return str;
            }
            return base.GetPropertyValue(s);
        }

        public override GSCore ExecuteMessage(string token, GSCoreCollection args)
        {
            if (token.Equals("replace"))
            {
                string a1 = args.getSafe(0).getStringValue();
                string a2 = args.getSafe(1).getStringValue();
                if (a1.Length > 0)
                {
                    return new GSString(Value.Replace(a1, a2));
                }
                else
                {
                    return this;
                }
            }
            else if (token.Equals("substring"))
            {
                int a = (int)args.getSafe(0).getIntegerValue();
                int b = (int)args.getSafe(1).getIntegerValue();
                if (b == 0)
                {
                    return new GSString(Value.Substring(a));
                }
                else
                {
                    return new GSString(Value.Substring(a, b));
                }
            }
            else if (token.Equals("padLeft"))
            {
                int a = (int)args.getSafe(0).getIntegerValue();
                if (a > Value.Length)
                    return new GSString(Value.PadLeft(a));
                else
                    return this;
            }
            else if (token.Equals("padRight"))
            {
                int a = (int)args.getSafe(0).getIntegerValue();
                if (a > Value.Length)
                    return new GSString(Value.PadRight(a));
                else
                    return this;
            }
            else
                return base.ExecuteMessage(token, args);
        }
        public override string ToString()
        {
            return Value;
        }

        public string StringToSafe(string s)
        {
            return s.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\t", "\\t").Replace("\"", "\\\"");
        }

        public override void writeScript(int level, System.IO.StreamWriter sw)
        {
            sw.Write("\"" + StringToSafe(Value)+ "\"");
        }
    }
}
