using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base.Scripting
{
    public class GSReturn: GSCore
    {
        // 0 - return
        // 1 - break
        // 2 - continue
        public const int TYPE_RETURN = 0;
        public const int TYPE_BREAK = 1;
        public const int TYPE_CONTINUE = 2;

        public int Type = 0;
        public GSCore Value = null;

        public GSReturn()
        {
        }

        public GSReturn(GSCore val)
        {
            Value = val;
        }

        public GSReturn(int type)
        {
            Type = type;
        }

        public GSReturn(int type, GSCore val)
        {
            Value = val;
            Type = type;
        }

        public override bool getBooleanValue()
        {
            if (Value != null)
                return Value.getBooleanValue();
            return base.getBooleanValue();
        }

        public override long getIntegerValue()
        {
            if (Value != null)
                return Value.getIntegerValue();
            return base.getIntegerValue();
        }

        public override string getStringValue()
        {
            if (Value != null)
                return Value.getStringValue();
            return base.getStringValue();
        }

        public override double getDoubleValue()
        {
            if (Value != null)
                return Value.getDoubleValue();
            return base.getDoubleValue();
        }
    }
}
