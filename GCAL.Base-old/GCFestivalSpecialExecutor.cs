using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public class GCFestivalSpecialExecutor: GSExecutor
    {
        private TResultCalendar calendar = null;

        private VAISNAVADAY defaultDay = new VAISNAVADAY();

        public GCFestivalSpecialExecutor(TResultCalendar cal)
        {
            calendar = cal;
            CurrentIndex = TResultCalendar.BEFORE_DAYS;
        }

        public override GSCore GetPropertyValue(string token)
        {
            if (token.Equals("today"))
            {
                return day(0);
            }
            else if (token.Equals("yesterday"))
            {
                return day(-1);
            }
            else if (token.Equals("tomorrow"))
            {
                return day(1);
            }
            else
            {
                return base.GetPropertyValue(token);
            }
        }
        public override GSCore ExecuteMessage(string token, GSCoreCollection args)
        {
            if (token.Equals("day"))
            {
                return day((int)args.getSafe(0).getIntegerValue());
            }
            else
            {
                return base.ExecuteMessage(token, args);
            }
        }

        /// <summary>
        /// Value from range 0...m_PureCount
        /// </summary>
        public int CurrentIndex { get; set; }

        /// <summary>
        /// Gets day relative to current day
        /// that means day(0) return current day (today)
        /// day(-1) returns previous day (yesterday)
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public VAISNAVADAY day(int offset)
        {
            int idx = CurrentIndex + offset;
            if (idx < 0 || idx >= calendar.m_nCount)
                return defaultDay;
            return calendar.m_pData[idx];
        }
    }
}
