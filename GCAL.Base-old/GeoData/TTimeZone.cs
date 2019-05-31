using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Xml;

namespace GCAL.Base
{
    public class TTimeZone
    {
        public bool Valid { get; set; }
        public string Name;
        public int OffsetMinutes;
        public int BiasMinutes;
        public TTimeZoneDst StartDst = new TTimeZoneDst();
        public TTimeZoneDst EndDst = new TTimeZoneDst();
        public string CountryISOCode { get; set; }

        public static List<TTimeZone> TimeZoneList = new List<TTimeZone>();
        public static bool Modified = false;
        private static TTimeZone p_defaultTimezone = null;


        public TTimeZone()
        {
            Valid = false;
            CountryISOCode = string.Empty;
        }

        public override string ToString()
        {
            return GetTimeZoneOffsetText(OffsetMinutes/60.0) + " " + Name;
        }

        public string EncodedString
        {
            get
            {
                return string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}",
                    Name, OffsetMinutes, BiasMinutes,
                    StartDst.Type, StartDst.Month, StartDst.Week, StartDst.Day,
                    EndDst.Type, EndDst.Month, EndDst.Week, EndDst.Day, 
                    CountryISOCode);
            }
            set
            {
                string[] fields = value.Split('\t');
                if (fields.Length < 9)
                {
                    Valid = false;
                    return;
                }

                Name = GCFestivalBase.SafeToString(fields[0]);
                OffsetMinutes = int.Parse(fields[1]);
                BiasMinutes = int.Parse(fields[2]);
                Valid = true;
                StartDst.Type = int.Parse(fields[3]);
                StartDst.Month = int.Parse(fields[4]);
                StartDst.Week = int.Parse(fields[5]);
                StartDst.Day = int.Parse(fields[6]);
                EndDst.Type = int.Parse(fields[7]);
                EndDst.Month = int.Parse(fields[8]);
                EndDst.Week = int.Parse(fields[9]);
                EndDst.Day = int.Parse(fields[10]);
                CountryISOCode = fields[11];
            }
        }

        public void SaveToNode(XmlElement e)
        {
            XmlElement ea;

            e.SetAttribute("Name", Name);
            e.SetAttribute("Offset", OffsetMinutes.ToString());
            e.SetAttribute("Bias", BiasMinutes.ToString());

            ea = e.OwnerDocument.CreateElement("StartDst");
            e.AppendChild(ea);
            StartDst.SaveToNode(ea);

            ea = e.OwnerDocument.CreateElement("EndDst");
            e.AppendChild(ea);
            EndDst.SaveToNode(ea);
        }

        public void LoadFromNode(XmlElement e)
        {
            Name = XH.GetXmlString(e, "Name", "(Unknown)");
            OffsetMinutes = XH.GetXmlInt(e, "Offset", 0);
            BiasMinutes = XH.GetXmlInt(e, "Bias", 0);
            StartDst.LoadFromNode(e["StartDst"]);
            EndDst.LoadFromNode(e["EndDst"]);
        }

        public static void SaveFile(string filePath)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement e1 = doc.CreateElement("TimeZones");
            doc.AppendChild(e1);

            foreach (TTimeZone timezone in TimeZoneList)
            {
                XmlElement e = doc.CreateElement("TimeZone");
                e1.AppendChild(e);
                timezone.SaveToNode(e);
            }

            doc.Save(filePath);
        }

        public static int LoadFile(string filePath)
        {
            TTimeZone pce;

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, Properties.Resources.timezones2016);
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            TimeZoneList.Clear();
            foreach (XmlElement e in doc.GetElementsByTagName("TimeZone"))
            {
                TTimeZone tz = new TTimeZone();
                tz.LoadFromNode(e);
                TimeZoneList.Add(tz);
            }

            return TimeZoneList.Count;
        }

        public string GetXMLString()
        {
            return string.Format("\t<dayss name=\"{0}\" types=\"{1}\" months=\"{2}\" weeks=\"{3}\" days=\"{4}\"\n\t\ttypee=\"{5}\" monthe=\"{6}\" weeke=\"{7}\" daye=\"{8}\" shift=\"{9}\"/>\n"
                        , Name, StartDst.Type, StartDst.Month, StartDst.Week, StartDst.Day,
                         EndDst.Type, EndDst.Month, EndDst.Week, EndDst.Day, BiasMinutes);

        }

        public int GetDaylightTimeStartDate(int nYear, ref GregorianDateTime vcStart)
        {
            vcStart.day = 1;
            vcStart.month = Convert.ToInt32(StartDst.Month);
            vcStart.year = nYear;
            if (StartDst.Type == 1)
            {
                vcStart.day = StartDst.Day;
            }
            else
            {
                if (StartDst.Month == 5)
                {
                    vcStart.day = GregorianDateTime.GetMonthMaxDays(nYear, StartDst.Month);
                    vcStart.InitWeekDay();
                    while (vcStart.dayOfWeek != StartDst.Day)
                    {
                        vcStart.PreviousDay();
                        vcStart.dayOfWeek = (vcStart.dayOfWeek + 6) % 7;
                    }
                }
                else
                {
                    vcStart.day = 1;
                    vcStart.InitWeekDay();
                    while (vcStart.dayOfWeek != StartDst.Day)
                    {
                        vcStart.NextDay();
                        vcStart.dayOfWeek = (vcStart.dayOfWeek + 1) % 7;
                    }
                    vcStart.day += StartDst.Week * 7;
                }
            }
            vcStart.shour = 1 / 24.0;
            return 0;
        }

        public int GetNormalTimeStartDate(int nYear, ref GregorianDateTime vcStart)
        {
            vcStart.day = 1;
            vcStart.month = 10;
            vcStart.year = nYear;
            vcStart.shour = 3 / 24.0;
            return 0;
        }

        public static string GetTimeZoneOffsetText(double d)
        {
            int a4, a5;
            int sig;

            if (d < 0.0)
            {
                sig = -1;
                d = -d;
            }
            else
            {
                sig = 1;
            }
            a4 = GCMath.IntFloor(d);
            a5 = Convert.ToInt32((d - a4) * 60);

            return string.Format("{0}{1}:{2:00}", (sig > 0 ? '+' : '-'), a4, a5);
        }

        public static string GetTimeZoneOffsetTextArg(double d)
        {
            int a4, a5;
            int sig;

            if (d < 0.0)
            {
                sig = -1;
                d = -d;
            }
            else
            {
                sig = 1;
            }
            a4 = Convert.ToInt32(d);
            a5 = Convert.ToInt32((d - a4) * 60 + 0.5);

            return string.Format("{0}{1}{2:00}", a4, (sig > 0 ? 'E' : 'W'), a5);
        }


        // return values
        // 0 - DST is off, yesterday was off
        // 1 - DST is on, yesterday was off
        // 2 - DST is on, yesterday was on
        // 3 - DST is off, yesterday was on
        public DstTypeChange DetermineDaylightChange(GregorianDateTime vc2)
        {
            int t2 = this.GetBiasMinutesForDay(vc2);
            GregorianDateTime vc3 = new GregorianDateTime();
            vc3.Set(vc2);
            vc3.PreviousDay();
            int t1 = this.GetBiasMinutesForDay(vc3);
            if (t1 != 0)
            {
                if (t2 != 0)
                    return DstTypeChange.DstOn;
                else
                    return DstTypeChange.DstEnd;
            }
            else if (t2 != 0)
            {
                return DstTypeChange.DstStart;
            }
            else
                return DstTypeChange.DstOff;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vc"></param>
        /// <returns>returns 1 if given date has bias</returns>
        public int GetBiasMinutesForDay(GregorianDateTime vc)
        {
            int biasExists = BiasMinutes;
            int biasZero = 0;

            if (vc.month == StartDst.Month)
            {
                if (StartDst.Type == 0)
                    return vc.IsEqualOrAfterWeekdayInWeek(StartDst.Week, StartDst.Day) ? biasExists : biasZero;
                else
                    return (vc.day >= StartDst.Day) ? biasExists : biasZero;
            }
            else if (vc.month == EndDst.Month)
            {
                if (EndDst.Type == 0)
                    return vc.IsEqualOrAfterWeekdayInWeek(EndDst.Week, EndDst.Day) ? biasZero : biasExists;
                else
                    return (vc.day >= EndDst.Day) ? biasZero : biasExists;
            }
            else if (StartDst.Month > EndDst.Month)
            {
                // zaciatocny mesiac ma vyssie cislo nez koncovy
                // napr. pre australiu
                if ((vc.month > StartDst.Month) || (vc.month < EndDst.Month))
                    return biasExists;
            }
            else
            {
                // zaciatocny mesiac ma nizsie cislo nez koncovy
                // usa, europa, asia
                if ((vc.month > StartDst.Month) && (vc.month < EndDst.Month))
                    return biasExists;
            }

            return biasZero;
        }


        public string HumanDstText()
        {
            string ret;

            if (BiasMinutes == 0)
            {
                ret = GCStrings.getString(807);
            }
            else
            {
                ret = GCStrings.getString(808);
                // pre datumovy den
                if (StartDst.Type == 1)
                {

                    ret += string.Format("from {0} {1} ", GCStrings.getString(810 + StartDst.Day), GCStrings.getString(794 + StartDst.Month));
                }
                else
                {
                    // pre tyzdenny den
                    ret += string.Format("from {0} {1} {2} ", GCStrings.getString(781 + StartDst.Week), GCStrings.getString(787 + StartDst.Day),
                        GCStrings.getString(794 + StartDst.Month));
                }

                if (EndDst.Type == 1)
                {
                    ret += string.Format("to {0} {1}", GCStrings.getString(810 + EndDst.Day), GCStrings.getString(794 + EndDst.Month));
                }
                else
                {
                    // pre tyzdenny den
                    ret += string.Format("to {0} {1} {2}", GCStrings.getString(781 + EndDst.Week),
                        GCStrings.getString(787 + EndDst.Day), GCStrings.getString(794 + EndDst.Month));
                }
            }

            return ret;
        }

        public static TTimeZone FindTimeZoneByName(string p_timezonename)
        {
            foreach (TTimeZone timeZone in TTimeZone.TimeZoneList)
            {
                if (timeZone.Name.Equals(p_timezonename))
                    return timeZone;
            }

            return GetDefaultTimeZone();
        }

        public static TTimeZone GetDefaultTimeZone()
        {
            if (p_defaultTimezone == null)
            {
                p_defaultTimezone = new TTimeZone();
                p_defaultTimezone.Name = "Undefined";
            }
            return p_defaultTimezone;
        }
    }

    /// <summary>
    /// Structure for storing data about start or end of daylight saving time system
    /// </summary>
    public class TTimeZoneDst
    {
        /// <summary>
        /// type of day, 0-day is given as n-th x-day of month, 1- day is given as DATE
        /// </summary>
        public int Type;
        /// <summary>
        /// Used only for StartDstType = 0, this is 
        /// number of week in which day occurs (1,2,3,4 is acceptable, 5 means *last*)
        /// </summary>
        public int Week;
        /// <summary>
        /// Month: 1,2..12
        /// </summary>
        public int Month;
        /// <summary>
        /// For StartDstType = 0 : 0 - sunday, 1 - monday, ... 6 - saturday
        /// For StartDstType = 1 : 1..31
        /// </summary>
        public int Day;

        public TTimeZoneDst()
        {
            Type = 0;
            Week = 0;
            Month = 0;
            Day = 0;
        }

        public int IntegerValue
        {
            get
            {
                return (Type << 24) + (Week << 16) + (Month << 8) + Day;
            }
        }

        public void Clear()
        {
            Type = 0;
            Week = 0;
            Month = 0;
            Day = 0;
        }

        public void SaveToNode(XmlElement e)
        {
            XH.SetXmlInt(e, "Type", Type);
            XH.SetXmlInt(e, "Week", Week);
            XH.SetXmlInt(e, "Month", Month);
            XH.SetXmlInt(e, "Day", Day);
        }

        public void LoadFromNode(XmlElement e)
        {
            Type = XH.GetXmlInt(e, "Type", 0);
            Week = XH.GetXmlInt(e, "Week", 0);
            Month = XH.GetXmlInt(e, "Month", 0);
            Day = XH.GetXmlInt(e, "Day", 0);
        }

    }

    public class XH
    {
        public static void SetXmlInt(XmlElement e, string attributeName, int value)
        {
            if (e != null)
                e.SetAttribute(attributeName, value.ToString());
        }

        public static int GetXmlInt(XmlElement e, string attributeName, int defaultValue)
        {
            if (e != null && e.HasAttribute(attributeName))
            {
                int value;
                if (int.TryParse(e.GetAttribute(attributeName), out value))
                    return value;
            }

            return defaultValue;
        }
        public static void SetXmlDouble(XmlElement e, string attributeName, double value)
        {
            if (e != null)
                e.SetAttribute(attributeName, value.ToString());
        }

        public static double GetXmlDouble(XmlElement e, string attributeName, double defaultValue)
        {
            if (e != null && e.HasAttribute(attributeName))
            {
                double value;
                if (double.TryParse(e.GetAttribute(attributeName), out value))
                    return value;
            }

            return defaultValue;
        }
        public static void SetXmlString(XmlElement e, string attributeName, string value)
        {
            if (e != null)
                e.SetAttribute(attributeName, value);
        }

        public static string GetXmlString(XmlElement e, string attributeName, string defaultValue)
        {
            if (e != null && e.HasAttribute(attributeName))
            {
                return e.GetAttribute(attributeName);
            }

            return defaultValue;
        }
    }

    public enum DstTypeChange
    {
        DstOn = 2,
        DstOff = 0,
        DstStart = 1,
        DstEnd = 3
    }
}
