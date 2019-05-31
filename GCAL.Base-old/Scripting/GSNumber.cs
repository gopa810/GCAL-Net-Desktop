using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base.Scripting
{
    public class GSNumber: GSCore
    {
        public enum NumberType
        {
            Int,
            Double
        }

        private double dValue = 0.0;
        private long lValue = 0L;
        private NumberType nType = NumberType.Int;

        public GSNumber()
        {
        }

        public GSNumber(int i)
        {
            IntegerValue = i;
        }

        public GSNumber(long l)
        {
            IntegerValue = l;
        }

        public GSNumber(double d)
        {
            DoubleValue = d;
        }

        public override string ToString()
        {
            return lValue.ToString() + " " + dValue.ToString();
        }

        public long IntegerValue
        {
            get
            {
                return lValue;
            }
            set
            {
                lValue = value;
                nType = NumberType.Int;
                dValue = Convert.ToDouble(value);
            }
        }

        public double DoubleValue
        {
            get
            {
                return dValue;
            }
            set
            {
                dValue = value;
                nType = NumberType.Double;
                lValue = Convert.ToInt64(value);
            }
        }

        public override string getStringValue()
        {
            if (nType == NumberType.Int)
                return lValue.ToString();
            else
                return dValue.ToString();
        }

        public override long getIntegerValue()
        {
            return lValue;
        }

        public override bool getBooleanValue()
        {
            return lValue != 0;
        }

        public override double getDoubleValue()
        {
            return dValue;
        }

        public bool IsInteger { get { return nType == NumberType.Int; } }
        public bool IsDouble { get { return nType == NumberType.Double; } }

        public override void writeScript(int level, System.IO.StreamWriter sw)
        {
            if (nType == NumberType.Int)
                sw.Write(lValue.ToString());
            else
                sw.Write(dValue.ToString());
        }
    }
}
