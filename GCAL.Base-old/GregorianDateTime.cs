using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public class GregorianDateTime: GSCore
    {
        public int year;
        public int month;
        public int dayOfWeek;
        public int day;
        public double shour;
        public double TimezoneHours;

        public GregorianDateTime()
        {
            DateTime dt = DateTime.Now;
            year = dt.Year;
            month = dt.Month;
            dayOfWeek = (int)dt.DayOfWeek;
            day = dt.Day;
            shour = 0.5;
            TimezoneHours = 0.0;
        }

        public GregorianDateTime(GregorianDateTime a)
        {
            Set(a);
        }


        public GregorianDateTime(int nYear, int nMonth, int nDay)
        {
            year = nYear;
            month = nMonth;
            day = nDay;
            InitWeekDay();
            shour = 0.5;
            TimezoneHours = 0.0;
        }

        public GregorianDateTime(GregorianDateTime a, int daysOffset)
        {
            Set(a);
            AddDays(daysOffset);
        }


        public string LongTime
        {
            get
            {
                return LongTimeString();
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2:0000}", day, GregorianDateTime.GetMonthAbreviation(month), year);
        }

        public override GSCore GetPropertyValue(string Token)
        {
            GSCore result = null;
            switch (Token)
            {
                case "day":
                    result = new GSNumber() { IntegerValue = day };
                    break;
                case "month":
                    result = new GSNumber() { IntegerValue = month };
                    break;
                case "monthName":
                    result = new GSString() { Value = GetMonthName(month) };
                    break;
                case "monthAbbr":
                    result = new GSString() { Value = GetMonthAbreviation(month) };
                    break;
                case "year":
                    result = new GSNumber() { IntegerValue = year };
                    break;
                case "weekday":
                    result = new GSNumber(dayOfWeek);
                    break;
                case "dayOfWeekName":
                    result = new GSString() { Value = GCStrings.getString(dayOfWeek) };
                    break;
                case "dayOfWeekAbbr":
                    result = new GSString() { Value = GCStrings.getString(dayOfWeek).Substring(0,2) };
                    break;
                case "dateWithExt":
                    result = new GSString() { Value = GetDateTextWithTodayExt(this) };
                    break;
                case "shortDate":
                    result = new GSString(ToString());
                    break;
                case "shortTime":
                    result = new GSString(ShortTimeString());
                    break;
                case "longTime":
                    result = new GSString(LongTimeString());
                    break;
                case "standardDateString":
                    result = new GSString(string.Format("{0:0000}{1:00}{2:00}", year, month, day));
                    break;
                case "standardTimeString":
                    result = new GSString(string.Format("{0:00}{1:00}{2:00}", GetHour(), GetMinute(), GetSecond()));
                    break;
                default:
                    result = base.GetPropertyValue(Token);
                    break;
            }

            return result;
        }

        public override GSCore ExecuteMessage(string token, GSCoreCollection args)
        {
            GSCore result = GSCore.Void;
            switch (token)
            {
                case "setDay":
                    day = (int)args.getSafe(0).getIntegerValue();
                    break;
                case "setMonth":
                    month = (int)args.getSafe(0).getIntegerValue();
                    break;
                case "setYear":
                    year = (int)args.getSafe(0).getIntegerValue();
                    break;
                default:
                    return base.ExecuteMessage(token, args);
            }

            return result;
        }

        public override string getStringValue()
        {
            return ToString();
        }

        public bool IsLessThan(GregorianDateTime date)
        {
	        int y1, y2, m1, m2, d1, d2;
	        double h1, h2;
	        d1 = this.day;
	        d2 = date.day;
	        m1 = this.month;
	        m2 = date.month;
	        y1 = this.year;
	        y2 = date.year;
	        h1 = shour + TimezoneHours/24.0;
	        h2 = date.shour + date.TimezoneHours/24.0;

            NormalizeValues(ref y1, ref m1, ref d1, ref h1);
            NormalizeValues(ref y2, ref m2, ref d2, ref h2);

	        if (y1 > y2)
		        return false;
	        else if (y1 < y2)
		        return true;
	        else
	        {
		        if (m1 > m2)
			        return false;
		        else if (m1 < m2)
			        return true;
		        else
		        {
			        if (d1 < d2)
				        return true;
			        else 
				        return false;
		        }
	        }
        }

        public bool IsLessOrEqualTo(GregorianDateTime date)
        {
            int y1, y2, m1, m2, d1, d2;
            double h1, h2;

            d1 = this.day;
            d2 = date.day;
            m1 = this.month;
            m2 = date.month;
            y1 = this.year;
            y2 = date.year;
            h1 = shour + TimezoneHours / 24.0;
            h2 = date.shour + date.TimezoneHours / 24.0;

            NormalizeValues(ref y1, ref m1, ref d1, ref h1);
            NormalizeValues(ref y2, ref m2, ref d2, ref h2);

            if (y1 > y2)
                return false;
            else if (y1 < y2)
                return true;
            else
            {
                if (m1 > m2)
                    return false;
                else if (m1 < m2)
                    return true;
                else
                {
                    if (d1 <= d2)
                        return true;
                    else
                        return false;
                }
            }
        }

        public bool IsDateEqual(GregorianDateTime date)
        {
            int y1, y2, m1, m2, d1, d2;
            double h1, h2;

            d1 = this.day;
            d2 = date.day;
            m1 = this.month;
            m2 = date.month;
            y1 = this.year;
            y2 = date.year;
            h1 = shour + TimezoneHours / 24.0;
            h2 = date.shour + date.TimezoneHours / 24.0;

            NormalizeValues(ref y1, ref m1, ref d1, ref h1);
            NormalizeValues(ref y2, ref m2, ref d2, ref h2);

            return ((y1 == y2) && (m1 == m2) && (d1 == d2));

        }

        public void NormalizeValues(ref int y1, ref int m1, ref int d1, ref double h1)
        {

	        if (h1 < 0.0)
	        {
		        d1--;
		        h1 += 1.0;
	        }
	        else if (h1 >= 1.0)
	        {
		        h1 -= 1.0;
		        d1++;
	        }
	        if (d1 < 1)
	        {
		        m1--;
		        if (m1 < 1)
		        {
			        m1 = 12;
			        y1--;
		        }
		        d1 = GregorianDateTime.GetMonthMaxDays(y1, m1);
	        }
	        else if (d1 > GregorianDateTime.GetMonthMaxDays(y1, m1))
	        {
		        m1++;
		        if (m1 > 12)
		        {
			        m1 = 1;
			        y1++;
		        }
		        d1 = 1;
	        }

        }

	    private static int [] m_months_ovr = { 0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 }; 
	    private static int [] m_months = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 }; 

        public static int GetMonthMaxDays(int year, int month)
        {
	        if (GregorianDateTime.IsLeapYear(year))
		        return m_months_ovr[month];
	        else
		        return m_months[month];
        }

        public static bool IsLeapYear(int year)
        {
	        if ((year % 4) == 0)
	        {
		        if ((year % 100 == 0) && (year % 400 != 0))
			        return false;
		        else
			        return true;
	        }

	        return false;
        }

        public void ChangeTimeZone(double tZone)
        {
	        shour += (tZone - TimezoneHours)/24;
	        NormalizeValues(ref year, ref month, ref day, ref shour);
	        TimezoneHours = tZone;
        }

        public int GetJulianInteger()
        {
	        int yy = year - Convert.ToInt32((12 - month) / 10);
	        int mm = month + 9;

	        if (mm >= 12)
		        mm -= 12;

	        int k1, k2, k3;
	        int j;

	        k1 = Convert.ToInt32 (Math.Floor(365.25 * (yy + 4712)));
	        k2 = Convert.ToInt32 (Math.Floor(30.6 * mm + 0.5));
	        k3 = Convert.ToInt32 (Math.Floor(Math.Floor((double)(yy/100)+49)*.75))-38;
	        j = k1 + k2 + day + 59;
	        if (j > 2299160)
		        j -= k3;

	        return j;
        }

        public double GetJulian()
        {
	        return (double)GetJulianInteger();
        }

        public void NormalizeValues()
        {
	        NormalizeValues(ref year, ref month, ref day, ref shour);
        }

        public void PreviousDay()
        {
	        day--;
	        if (day < 1)
	        {
		        month--;
		        if (month < 1)
		        {
			        month = 12;
			        year--;
		        }
		        day = GregorianDateTime.GetMonthMaxDays(year, month);
	        }
	        dayOfWeek = (dayOfWeek + 6) % 7;
        }

        public void NextDay()
        {
	        day++;
	        if (day > GregorianDateTime.GetMonthMaxDays(year, month))
	        {
		        month++;
		        if (month > 12)
		        {
			        month = 1;
			        year++;
		        }
		        day = 1;
	        }
	        dayOfWeek = (dayOfWeek + 1) % 7;
        }

        public void AddDays(int n)
        {
            for (int i = 0; i < n; i++)
                NextDay();
        }

        public void SubtractDays(int n)
        {
            for (int i = 0; i < n; i++)
                PreviousDay();
        }


        /// <summary>
        /// Julian Date including day hours (GMT julian date time)
        /// </summary>
        /// <returns></returns>
        public double GetJulianDetailed()
        {
            return GetJulian() - 0.5 + shour;
        }

        /// <summary>
        /// Julian Date including day hours and timezone (local julian date time)
        /// </summary>
        /// <returns></returns>
        public double GetJulianComplete()
        {
            return GetJulian() - 0.5 + shour - TimezoneHours / 24.0;
        }

        public void InitWeekDay()
        {
	        dayOfWeek = (GetJulianInteger() + 1) % 7;
        }

        //***********************************************************************/
        //* Name:    calcDateFromJD								*/
        //* Type:    Function									*/
        //* Purpose: Calendar date from Julian Day					*/
        //* Arguments:										*/
        //*   jd   : Julian Day									*/
        //* Return value:										*/
        //*   String date in the form DD-MONTHNAME-YYYY					*/
        //* Note:											*/
        //***********************************************************************/

        public void SetFromJulian(double julianUtcDate)
        {
	        double z = Math.Floor(julianUtcDate + 0.5);

	        double f = (julianUtcDate + 0.5) - z;

	        double A, B, C, D, E, alpha;

	        if (z < 2299161.0)
	        {
		        A = z;
	        } 
	        else 
	        {
		        alpha = Math.Floor((z - 1867216.25)/36524.25);
		        A = z + 1.0 + alpha - Math.Floor(alpha/4.0);
	        }

	        B = A + 1524;
	        C = Math.Floor((B - 122.1)/365.25);
	        D = Math.Floor(365.25 * C);
	        E = Math.Floor((B - D)/30.6001);
	        day = Convert.ToInt32(Math.Floor(B - D - Math.Floor(30.6001 * E) + f));
	        month = Convert.ToInt32((E < 14) ? E - 1 : E - 13);
	        year = Convert.ToInt32((month > 2) ? C - 4716 : C - 4715);
	        TimezoneHours = 0.0;
	        shour = julianUtcDate + 0.5 - Math.Floor(julianUtcDate + 0.5);
        }

        public void Clear()
        {
            year = 0;
            month = 0;
            day = 0;
            shour = 0.0;
            TimezoneHours = 0.0;
        }

        public void Set(GregorianDateTime ta)
        {
            year = ta.year;
            month = ta.month;
            day = ta.day;
            shour = ta.shour;
            TimezoneHours = ta.TimezoneHours;
            dayOfWeek = ta.dayOfWeek;
        }

        public bool IsBeforeThis(GregorianDateTime date)
        {
            int y1, y2, m1, m2, d1, d2;
            d1 = this.day;
            d2 = date.day;
            m1 = this.month;
            m2 = date.month;
            y1 = this.year;
            y2 = date.year;

            if (y1 > y2)
                return false;
            else if (y1 < y2)
                return true;
            else if (m1 > m2)
                return false;
            else if (m1 < m2)
                return true;
            else if (d1 < d2)
                return true;
            else
                return false;
        }

        public void Today()
        {
            DateTime st = DateTime.Now;

            day = st.Day;
            month = st.Month;
            year = st.Year;
            shour = 0.5;
            TimezoneHours = 0;

            TimeZoneInfo tzi = TimeZoneInfo.Local;
            TimezoneHours = tzi.BaseUtcOffset.Minutes / 60.0;
        }

        public int CompareYMD(GregorianDateTime v)
        {
	        if (v.year < year)
		        return (year - v.year) * 365;
	        else if (v.year > year)
		        return (year - v.year) * 365;

	        if (v.month < month)
		        return (month - v.month)*31;
	        else if (v.month > month)
		        return (month - v.month)*31;

	        return (day - v.day);
        }

        public int GetHour()
        {
            return Convert.ToInt32(Math.Floor(shour * 24));
        }

        public int GetMinute()
        {
            return Convert.ToInt32(Math.Floor((shour * 24 - Math.Floor(shour * 24)) * 60));
        }

        public int GetMinuteRound()
        {
            return Convert.ToInt32(Math.Floor((shour * 24 - Math.Floor(shour * 24)) * 60 + 0.5));
        }

        public int GetSecond()
        {
            return Convert.ToInt32(Math.Floor((shour * 1440 - Math.Floor(shour * 1440)) * 60));
        }

        public int GetDayInteger()
        {
            return year * 384 + month * 32 + day;
        }

        public string c_str()
        {
	        return string.Format("{0:##} {1} {2}  {3:00}:{4:00}:{5:00}", day, GregorianDateTime.GetMonthAbreviation(month), year, GetHour(), GetMinute(), GetSecond());
        }

        public static string GetDateTextWithTodayExt(GregorianDateTime vc)
        {
	        if ((vc.day > 0) && (vc.day < 32) && (vc.month > 0) && (vc.month < 13) && (vc.year >= 1500) && (vc.year < 4000))
	        {
		        GregorianDateTime today = new GregorianDateTime();
		        int diff = today.GetJulianInteger() - vc.GetJulianInteger();
		        if (diff == 0)
		        {
			        return string.Format("{0} {1} {2} ({3})", vc.day, GregorianDateTime.GetMonthAbreviation(vc.month), vc.year, GCStrings.Localized("Today"));
		        }
		        else if (diff == -1)
		        {
			        return string.Format("{0} {1} {2} ({3})", vc.day, GregorianDateTime.GetMonthAbreviation(vc.month), vc.year, GCStrings.Localized("Tomorrow"));
		        }
		        else if (diff == 1)
		        {
			        return string.Format("{0} {1} {2} ({3})", vc.day, GregorianDateTime.GetMonthAbreviation(vc.month), vc.year, GCStrings.Localized("Yesterday"));
		        }
	        }
            return string.Format("{0} {1} {2}", vc.day, GregorianDateTime.GetMonthAbreviation(vc.month), vc.year);
        }

        public string Format(string format)
        {
            StringBuilder sb = new StringBuilder(format);

            if (format.IndexOf("{day}") >= 0)
                sb.Replace("{day}", day.ToString());
            if (format.IndexOf("{month}") >= 0)
                sb.Replace("{month}", month.ToString());
            if (format.IndexOf("{monthAbr}") >= 0)
                sb.Replace("{monthAbr}", GetMonthAbreviation(month));
            if (format.IndexOf("{monthName}") >= 0)
                sb.Replace("{monthName}", GetMonthName(month));
            if (format.IndexOf("{hour}") >= 0)
                sb.Replace("{hour}", GetHour().ToString("D2"));
            if (format.IndexOf("{min}") >= 0)
                sb.Replace("{min}", GetMinute().ToString("D2"));
            if (format.IndexOf("{minRound}") >= 0)
                sb.Replace("{minRound}", GetMinuteRound().ToString("D2"));
            if (format.IndexOf("{sec}") >= 0)
                sb.Replace("{sec}", GetSecond().ToString("D2"));
            if (format.IndexOf("{year}") >= 0)
                sb.Replace("{year}", year.ToString());

            return sb.ToString();
        }

        public static double CalculateJulianDay(int year, int month, int day)
        {
	        int yy = year - Convert.ToInt32((12 - month) / 10);
	        int mm = month + 9;

	        if (mm >= 12)
		        mm -= 12;

	        int k1, k2, k3;
	        int j;

	        k1 = Convert.ToInt32(Math.Floor(365.25 * (yy + 4712)));
	        k2 = Convert.ToInt32(Math.Floor(30.6 * mm + 0.5));
	        k3 = Convert.ToInt32(Math.Floor(Math.Floor((double)(yy/100)+49)*.75))-38;
	        j = k1 + k2 + day + 59;
	        if (j > 2299160)
		        j -= k3;

	        return Convert.ToDouble(j);
        }

        public string ShortTimeString()
        {
            int h, m;
            GCMath.DaytimeToHourMin(shour, out h, out m);
            return string.Format("{0:00}:{1:00}", h, m);
        }

        public string LongTimeString()
        {
            int h, m, s;
            GCMath.DaytimeToHourMin(shour, out h, out m, out s);
            return string.Format("{0:00}:{1:00}:{2:00}", h, m, s);
        }

        public static string TimeSpanToLongString(long seconds)
        {
            int h, m, s;
            s = (int)(seconds % 60);
            m = (int)((seconds / 60) % 60);
            h = (int)(seconds / 3600);
            return string.Format("{0:00}:{1:00}:{2:00}", h, m, s);
        }

        public string FullString()
        {
            return string.Format("{0} {1}", c_str(), ShortTimeString());
        }

        private static string[] p_monthName = new string[] {
            "January",   // GCStrings.Localized("January");
            "February",  // GCStrings.Localized("February");
            "March",     // GCStrings.Localized("March");
            "April",     // GCStrings.Localized("April");
            "May",       // GCStrings.Localized("May");
            "June",      // GCStrings.Localized("June");
            "July",      // GCStrings.Localized("July");
            "August",    // GCStrings.Localized("August");
            "September", // GCStrings.Localized("September");
            "October",   // GCStrings.Localized("October");
            "November",  // GCStrings.Localized("November");
            "December"   // GCStrings.Localized("December");
        };

        private static string[] p_monthNameAbbr = new string[] {
            "Jan", // GCStrings.Localized("Jan");
            "Feb", // GCStrings.Localized("Feb");
            "Mar", // GCStrings.Localized("Mar");
            "Apr", // GCStrings.Localized("Apr");
            "May", // GCStrings.Localized("May");
            "Jun", // GCStrings.Localized("Jun");
            "Jul", // GCStrings.Localized("Jul");
            "Aug", // GCStrings.Localized("Aug");
            "Sep", // GCStrings.Localized("Sep");
            "Oct", // GCStrings.Localized("Oct");
            "Nov", // GCStrings.Localized("Nov");
            "Dec"  // GCStrings.Localized("Dec");
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="month">Month from range 1..12</param>
        /// <returns></returns>
        public static string GetMonthName(int month)
        {
            return GCStrings.Localized(p_monthName[(month - 1) % 12]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="month">Month from range 1..12</param>
        /// <returns></returns>
        public static string GetMonthAbreviation(int month)
        {
            return GCStrings.Localized(p_monthNameAbbr[(month - 1) % 12]);
        }

        public string EncodedString
        {
            get
            {
                return string.Format("{0}|{1}|{2}|{3}|{4}|{5}", year, month, day, shour, TimezoneHours, dayOfWeek);
            }
            set
            {
                if (value != null)
                {
                    string[] a = value.Split('|');
                    if (a.Length == 6)
                    {
                        int Y, M, D, DOW;
                        double SH, TZ;
                        if (int.TryParse(a[0], out Y)
                            && int.TryParse(a[1], out M)
                            && int.TryParse(a[2], out D)
                            && double.TryParse(a[3], out SH)
                            && double.TryParse(a[4], out TZ)
                            && int.TryParse(a[5], out DOW))
                        {
                            year = Y;
                            month = M;
                            day = D;
                            dayOfWeek = DOW;
                            shour = SH;
                            TimezoneHours = TZ;
                        }
                    }
                }
            }
        }


        public GregorianDateTime TimeWithOffset(double p)
        {
            GregorianDateTime dt = new GregorianDateTime(this);
            dt.shour += p;
            dt.NormalizeValues();
            return dt;
        }

        public static int Compare(GregorianDateTime A, GregorianDateTime B)
        {
            if (A.year < B.year)
                return -1;
            else if (A.year > B.year)
                return 1;

            if (A.month < B.month)
                return -1;
            else if (A.month > B.month)
                return 1;

            if (A.day < B.day)
                return -1;
            else if (A.day > B.day)
                return 1;

            if (A.shour < B.shour)
                return -1;
            else if (A.shour > B.shour)
                return 1;

            return 0;

        }

        /// <summary>
        /// Adding hours to datetime
        /// </summary>
        /// <param name="ndst">Number of hours. For example 1.5 means 1 hour and 30 minutes</param>
        public void AddHours(double ndst)
        {
            shour += ndst / 24.0;
            NormalizeValues();
        }

        /// <summary>
        /// Tests whether this date is equal or after the date, when given weekday in given week occurs.
        /// </summary>
        /// <param name="weekNumber">Number of week. values 1, 2, 3, 4 are order of week, value 5 means last week of the month</param>
        /// <param name="dayNumber">Number of weekday, value 0 is Sunday, 1 is Monday, .... 6 is Saturday</param>
        /// <returns>Returns true, if this date is equal or is after the date given by weeknumber and daynumber</returns>
        public bool IsEqualOrAfterWeekdayInWeek(int weekNumber, int dayNumber)
        {
            GregorianDateTime vc = this;
            int[] xx = { 1, 7, 6, 5, 4, 3, 2 };

            int dowFirstDay, firstGivenWeekday, requiredGivenWeekday, lastDayInMonth;

            // prvy den mesiaca
            dowFirstDay = xx[(7 + vc.day - vc.dayOfWeek) % 7];

            // 1. x-day v mesiaci ma datum
            firstGivenWeekday = xx[(dowFirstDay - dayNumber + 7) % 7];

            // n-ty x-day ma datum
            if ((weekNumber < 0) || (weekNumber >= 5))
            {
                requiredGivenWeekday = firstGivenWeekday + 28;
                lastDayInMonth = GregorianDateTime.GetMonthMaxDays(vc.year, vc.month);
                while (requiredGivenWeekday > lastDayInMonth)
                {
                    requiredGivenWeekday -= 7;
                }
            }
            else
            {
                requiredGivenWeekday = Convert.ToInt32(firstGivenWeekday + (weekNumber - 1) * 7);
            }

            return vc.day >= requiredGivenWeekday;
        }

        public bool EqualDay(GregorianDateTime that)
        {
            return this.day == that.day && this.month == that.month && this.year == that.year;
        }
    }
}
