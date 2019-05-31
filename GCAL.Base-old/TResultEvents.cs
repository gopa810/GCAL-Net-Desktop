﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public class TResultEvents: TResultBase
    {
        public const int SORTING_BY_TYPE = 0;
        public const int SORTING_BY_DATE = 1;

        public GregorianDateTime StartDateTime = new GregorianDateTime();
        public GregorianDateTime EndDateTime = new GregorianDateTime();
        public GCLocation EarthLocation = new GCLocation();
        public int SortType = SORTING_BY_DATE;

        public void Clear()
        {
            p_events.Clear();
        }

        public List<TDayEvent> p_events = new List<TDayEvent>();

        public override GSCore GetPropertyValue(string s)
        {
            if (s.Equals("items"))
            {
                GSList list = new GSList();
                foreach (TDayEvent de in p_events)
                {
                    list.Add(de);
                }
                return list;
            }
            else if (s.Equals("startDate"))
                return StartDateTime;
            else if (s.Equals("endDate"))
                return EndDateTime;
            else if (s.Equals("isSortedByTime"))
                return new GSBoolean(SortType == TResultEvents.SORTING_BY_DATE);
            else if (s.Equals("isSortedByType"))
                return new GSBoolean(SortType == TResultEvents.SORTING_BY_TYPE);
            else
            {
                return base.GetPropertyValue(s);
            }
        }

        public bool AddEvent(GregorianDateTime inTime, int inType, int inData, DstTypeChange inDst)
        {
            TDayEvent de = new TDayEvent();

            de.Time = new GregorianDateTime();
            de.Time.Set(inTime);

            switch (inDst)
            {
                case DstTypeChange.DstOff:
                    de.nDst = 0;
                    break;
                case DstTypeChange.DstStart:
                    if (de.Time.shour >= 2 / 24.0)
                    {
                        de.Time.shour += 1 / 24.0;
                        de.Time.NormalizeValues();
                        de.nDst = 1;
                    }
                    else
                    {
                        de.nDst = 0;
                    }
                    break;
                case DstTypeChange.DstOn:
                    de.Time.shour += 1 / 24.0;
                    de.Time.NormalizeValues();
                    de.nDst = 1;
                    break;
                case DstTypeChange.DstEnd:
                    if (de.Time.shour <= 2 / 24.0)
                    {
                        de.Time.shour += 1 / 24.0;
                        de.Time.NormalizeValues();
                        de.nDst = 1;
                    }
                    else
                    {
                        de.nDst = 0;
                    }
                    break;
                default:
                    de.nDst = 0;
                    break;
            }
            de.nData = inData;
            de.nType = inType;

            p_events.Add(de);

            return true;
        }

        public void Sort(int nSortType)
        {
            SortType = nSortType;
            TDayEventComparer dec = new TDayEventComparer();
            dec.SortType = nSortType;
            p_events.Sort(dec);
        }

        public TDayEvent this[int i]
        {
            get
            {
                return p_events[i];
            }
        }
        public TResultEvents()
        {
            SortType = SORTING_BY_DATE;
            p_events.Clear();
        }

        public void CalculateEvents(GCLocation loc, GregorianDateTime vcStart, GregorianDateTime vcEnd)
        {
            //GCSunData sun = new GCSunData();
            DstTypeChange ndst = 0;
            int nData;

            TResultEvents inEvents = this;
            this.Clear();
            this.EarthLocation = loc;
            this.StartDateTime = new GregorianDateTime(vcStart);
            this.EndDateTime = new GregorianDateTime(vcEnd);

            GregorianDateTime vc = new GregorianDateTime();
            GregorianDateTime vcAdd = new GregorianDateTime(), vcNext = new GregorianDateTime();
            GCEarthData earth = loc.GetEarthData();

            vc.Set(vcStart);
            vc.TimezoneHours = loc.OffsetUtcHours;
            vcAdd.Set(vc);
            vcAdd.InitWeekDay();

            GCHourTime sunriseData, sunsetData;
            double sunRise, sunSet;
            double r1, r2;

            while (vcAdd.IsBeforeThis(vcEnd))
            {
                sunriseData = GCSunData.CalcSunrise(vcAdd, earth);
                sunsetData = GCSunData.CalcSunset(vcAdd, earth);
                sunRise = sunriseData.TotalDays;
                sunSet = sunsetData.TotalDays;
                ndst = loc.TimeZone.DetermineDaylightChange(vcAdd);

                if (GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_SUN) != 0)
                {
                    ndst = loc.TimeZone.DetermineDaylightChange(vcAdd);

                    vcAdd.shour = sunriseData.TotalDays - 96.0/1440.0;
                    inEvents.AddEvent(vcAdd, CoreEventType.CCTYPE_S_ARUN, 0, ndst);

                    vcAdd.shour = sunRise;
                    inEvents.AddEvent(vcAdd, CoreEventType.CCTYPE_S_RISE, 0, ndst);

                    vcAdd.shour = (sunRise + sunSet) / 2;
                    inEvents.AddEvent(vcAdd, CoreEventType.CCTYPE_S_NOON, 0, ndst);

                    vcAdd.shour = sunSet;
                    inEvents.AddEvent(vcAdd, CoreEventType.CCTYPE_S_SET, 0, ndst);
                }

                /*if (GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_ASCENDENT) != 0)
                {
                    todayLongitude = sun.longitude_deg;
                    vcAdd.shour = sunRise;
                    todaySunriseHour = sunRise;
                    if (previousLongitude < -10)
                    {
                        GregorianDateTime prevSunrise = new GregorianDateTime();
                        prevSunrise.Set(vcAdd);
                        prevSunrise.PreviousDay();
                        sun.SunCalc(prevSunrise, earth);
                        previousSunriseHour = sun.rise.GetDayTime() - 1;
                        previousLongitude = sun.longitude_deg;
                        fromTimeLimit = 0;
                    }

                    double a, b;
                    double jd = vcAdd.GetJulianComplete();
                    double ayan = GCAyanamsha.GetAyanamsa(jd);
                    r1 = GCMath.putIn360(previousLongitude - ayan) / 30;
                    r2 = GCMath.putIn360(todayLongitude - ayan) / 30;

                    while (r2 > r1 + 13)
                    {
                        r2 -= 12.0;
                    }
                    while (r2 < r1 + 11)
                    {
                        r2 += 12.0;
                    }

                    a = (r2 - r1) / (todaySunriseHour - previousSunriseHour);
                    b = r2 - a * todaySunriseHour;

                    for (double tr = Math.Floor(r1) + 1.0; tr < r2; tr += 1.0)
                    {
                        double tm = (tr - b) / a;
                        if (tm > fromTimeLimit)
                        {
                            vcNext.Set(vcAdd);
                            vcNext.shour = tm;
                            vcNext.NormalizeValues();
                            inEvents.AddEvent(vcNext, CoreEventType.CCTYPE_ASCENDENT, (int)tr, ndst);
                        }
                    }

                    previousLongitude = todayLongitude;
                    previousSunriseHour = todaySunriseHour - 1;
                    fromTimeLimit = previousSunriseHour;
                }*/

                if (GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_RAHUKALAM) != 0)
                {
                    GCSunData.CalculateKala(sunRise, sunSet, vcAdd.dayOfWeek, out r1, out r2, KalaType.KT_RAHU_KALAM);

                    vcAdd.shour = r1;
                    inEvents.AddEvent(vcAdd, CoreEventType.CCTYPE_KALA_START, KalaType.KT_RAHU_KALAM, ndst);

                    vcAdd.shour = r2;
                    inEvents.AddEvent(vcAdd, CoreEventType.CCTYPE_KALA_END, KalaType.KT_RAHU_KALAM, ndst);
                }

                if (GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_YAMAGHANTI) != 0)
                {
                    GCSunData.CalculateKala(sunRise, sunSet, vcAdd.dayOfWeek, out r1, out r2, KalaType.KT_YAMA_GHANTI);

                    vcAdd.shour = r1;
                    inEvents.AddEvent(vcAdd, CoreEventType.CCTYPE_KALA_START, KalaType.KT_YAMA_GHANTI, ndst);

                    vcAdd.shour = r2;
                    inEvents.AddEvent(vcAdd, CoreEventType.CCTYPE_KALA_END, KalaType.KT_YAMA_GHANTI, ndst);
                }

                if (GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_GULIKALAM) != 0)
                {
                    GCSunData.CalculateKala(sunRise, sunSet, vcAdd.dayOfWeek, out r1, out r2, KalaType.KT_GULI_KALAM);

                    vcAdd.shour = r1;
                    inEvents.AddEvent(vcAdd, CoreEventType.CCTYPE_KALA_START, KalaType.KT_GULI_KALAM, ndst);

                    vcAdd.shour = r2;
                    inEvents.AddEvent(vcAdd, CoreEventType.CCTYPE_KALA_END, KalaType.KT_GULI_KALAM, ndst);
                }

                if (GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_ABHIJIT_MUHURTA) != 0)
                {
                    GCSunData.CalculateKala(sunRise, sunSet, vcAdd.dayOfWeek, out r1, out r2, KalaType.KT_ABHIJIT);

                    if (r1 > 0 && r2 > 0)
                    {
                        vcAdd.shour = r1;
                        inEvents.AddEvent(vcAdd, CoreEventType.CCTYPE_KALA_START, KalaType.KT_ABHIJIT, ndst);

                        vcAdd.shour = r2;
                        inEvents.AddEvent(vcAdd, CoreEventType.CCTYPE_KALA_END, KalaType.KT_ABHIJIT, ndst);
                    }
                }

                vcAdd.NextDay();
            }

            if (GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_ASCENDENT) != 0)
            {
                GCAscendant asc = new GCAscendant();
                asc.Earth = EarthLocation.GetEarthData();
                asc.CurrentDateTime = new GregorianDateTime(vc);
                while (asc.GetNextAscendantBefore(vcEnd))
                {
                    ndst = loc.TimeZone.DetermineDaylightChange(vcNext);
                    inEvents.AddEvent(asc.CurrentDateTime, CoreEventType.CCTYPE_ASCENDENT, asc.CurrentSign, ndst);
                    asc.CurrentDateTime.AddHours(0.5);
                }
            }

            if (GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_TITHI) != 0)
            {
                vcAdd.Set(vc);
                vcAdd.shour = 0.0;
                while (vcAdd.IsBeforeThis(vcEnd))
                {
                    nData = GCTithi.GetNextTithiStart(earth, vcAdd, out vcNext);
                    if (vcNext.GetDayInteger() < vcEnd.GetDayInteger())
                    {
                        vcNext.InitWeekDay();
                        ndst = loc.TimeZone.DetermineDaylightChange(vcNext);
                        inEvents.AddEvent(vcNext, CoreEventType.CCTYPE_TITHI, nData, ndst);
                    }
                    else
                    {
                        break;
                    }
                    vcAdd.Set(vcNext);
                    vcAdd.shour += 0.2;
                    if (vcAdd.shour >= 1.0)
                    {
                        vcAdd.shour -= 1.0;
                        vcAdd.NextDay();
                    }
                }
            }

            if (GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_NAKSATRA) != 0)
            {
                vcAdd.Set(vc);
                vcAdd.shour = 0.0;
                while (vcAdd.IsBeforeThis(vcEnd))
                {
                    nData = GCNaksatra.GetNextNaksatra(earth, vcAdd, out vcNext);
                    if (vcNext.GetDayInteger() < vcEnd.GetDayInteger())
                    {
                        vcNext.InitWeekDay();
                        ndst = loc.TimeZone.DetermineDaylightChange(vcNext);
                        inEvents.AddEvent(vcNext, CoreEventType.CCTYPE_NAKS, nData, ndst);
                    }
                    else
                    {
                        break;
                    }
                    vcAdd.Set(vcNext);
                    vcAdd.shour += 0.2;
                    if (vcAdd.shour >= 1.0)
                    {
                        vcAdd.shour -= 1.0;
                        vcAdd.NextDay();
                    }
                }
            }

            if (GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_YOGA) != 0)
            {
                vcAdd.Set(vc);
                vcAdd.shour = 0.0;
                while (vcAdd.IsBeforeThis(vcEnd))
                {
                    nData = GCYoga.GetNextYogaStart(earth, vcAdd, out vcNext);
                    if (vcNext.GetDayInteger() < vcEnd.GetDayInteger())
                    {
                        vcNext.InitWeekDay();
                        ndst = loc.TimeZone.DetermineDaylightChange(vcNext);
                        inEvents.AddEvent(vcNext, CoreEventType.CCTYPE_YOGA, nData, ndst);
                    }
                    else
                    {
                        break;
                    }
                    vcAdd.Set(vcNext);
                    vcAdd.shour += 0.2;
                    if (vcAdd.shour >= 1.0)
                    {
                        vcAdd.shour -= 1.0;
                        vcAdd.NextDay();
                    }
                }
            }

            if (GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_SANKRANTI) != 0)
            {
                vcNext = new GregorianDateTime();
                vcAdd.Set(vc);
                vcAdd.shour = 0.0;
                while (vcAdd.IsBeforeThis(vcEnd))
                {
                    vcNext.Set(GCSankranti.GetNextSankranti(vcAdd, earth, out nData));
                    if (vcNext.GetDayInteger() < vcEnd.GetDayInteger())
                    {
                        vcNext.InitWeekDay();
                        ndst = loc.TimeZone.DetermineDaylightChange(vcNext);
                        inEvents.AddEvent(vcNext, CoreEventType.CCTYPE_SANK, nData, ndst);
                    }
                    else
                    {
                        break;
                    }
                    vcAdd.Set(vcNext);
                    vcAdd.NextDay();
                }
            }

            if (GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_MOONRASI) != 0)
            {
                vcAdd.Set(vc);
                vcAdd.shour = 0.0;
                while (vcAdd.IsBeforeThis(vcEnd))
                {
                    nData = GCMoonData.GetNextMoonRasi(earth, vcAdd, out vcNext);
                    if (vcNext.GetDayInteger() < vcEnd.GetDayInteger())
                    {
                        vcNext.InitWeekDay();
                        ndst = loc.TimeZone.DetermineDaylightChange(vcNext);
                        inEvents.AddEvent(vcNext, CoreEventType.CCTYPE_M_RASI, nData, ndst);
                    }
                    else
                    {
                        break;
                    }
                    vcAdd.Set(vcNext);
                    vcAdd.shour += 0.5;
                    vcAdd.NormalizeValues();
                }

            }
            if (GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_CONJUNCTION) != 0)
            {
                double dlong;
                vcAdd.Set(vc);
                vcAdd.shour = 0.0;
                while (vcAdd.IsBeforeThis(vcEnd))
                {
                    dlong = GCConjunction.GetNextConjunction(vcAdd, out vcNext, true, earth);
                    if (vcNext.GetDayInteger() < vcEnd.GetDayInteger())
                    {
                        vcNext.InitWeekDay();
                        ndst = loc.TimeZone.DetermineDaylightChange(vcNext);
                        inEvents.AddEvent(vcNext, CoreEventType.CCTYPE_CONJ, GCRasi.GetRasi(dlong, GCAyanamsha.GetAyanamsa(vcNext.GetJulianComplete())), ndst);
                    }
                    else
                    {
                        break;
                    }
                    vcAdd.Set(vcNext);
                    vcAdd.NextDay();
                }
            }

            if (GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_MOON) != 0)
            {
                vcAdd.Set(vc);
                vcAdd.shour = 0.0;
                while (vcAdd.IsBeforeThis(vcEnd))
                {
                    vcNext.Set(GCMoonData.GetNextRise(earth, vcAdd, true));
                    inEvents.AddEvent(vcNext, CoreEventType.CCTYPE_M_RISE, 0, ndst);

                    vcNext.Set(GCMoonData.GetNextRise(earth, vcNext, false));
                    inEvents.AddEvent(vcNext, CoreEventType.CCTYPE_M_SET, 0, ndst);

                    vcNext.shour += 0.05;
                    vcNext.NormalizeValues();
                    vcAdd.Set(vcNext);
                }
            }

            if (GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_ASCENDENT) != 0)
            {/*
		        vcAdd = vc;
		        vcAdd.shour = 0.0;
		        while(vcAdd.IsBeforeThis(vcEnd))
		        {
			        nData = earth.GetNextAscendentStart(vcAdd, vcNext);
			        if (vcNext.GetDayInteger() < vcEnd.GetDayInteger())
			        {
				        vcNext.InitWeekDay();
				        ndst = TTimeZone.determineDaylightChange(vcNext, loc.m_nDST);
				        inEvents.AddEvent(vcNext, CCTYPE_ASCENDENT, nData, ndst);
			        }
			        else
			        {
				        break;
			        }
			        vcAdd = vcNext;
			        vcAdd.shour += 1/24.0;
			        if (vcAdd.shour >= 1.0)
			        {
				        vcAdd.shour -= 1.0;
				        vcAdd.NextDay();
			        }
		        }

		        */
            }

            if (GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_SORT) != 0)
                inEvents.Sort(SORTING_BY_DATE);
            else
                inEvents.Sort(SORTING_BY_TYPE);
        }

        public override string formatText(string df)
        {
            GSScript script = new GSScript();
            switch (df)
            {
                case GCDataFormat.PlainText:
                    script.readTextTemplate(Properties.Resources.TplCoreEventsPlain);
                    break;
                case GCDataFormat.Rtf:
                    script.readTextTemplate(Properties.Resources.TplCoreEventsRtf);
                    break;
                case GCDataFormat.HTML:
                    script.readTextTemplate(Properties.Resources.TplCoreEventsHtml);
                    break;
                case GCDataFormat.XML:
                    script.readTextTemplate(Properties.Resources.TplCoreEventsXml);
                    break;
                case GCDataFormat.CSV:
                    script.readTextTemplate(Properties.Resources.TplCoreEventsCsv);
                    break;
                default:
                    break;
            }


            GSExecutor engine = new GSExecutor();
            engine.SetVariable("events", this);
            engine.SetVariable("location", this.EarthLocation);
            engine.SetVariable("app", GCUserInterface.Shared);
            engine.ExecuteElement(script);

            return engine.getOutput();
        }

        public override TResultFormatCollection getFormats()
        {
            TResultFormatCollection coll = base.getFormats();

            coll.ResultName = "CoreEvents";
            coll.Formats.Add(new TResultFormat("Text File", "txt", GCDataFormat.PlainText));
            coll.Formats.Add(new TResultFormat("Rich Text File", "rtf", GCDataFormat.Rtf));
            coll.Formats.Add(new TResultFormat("XML File", "xml", GCDataFormat.XML));
            coll.Formats.Add(new TResultFormat("Comma Separated Values", "csv", GCDataFormat.CSV));
            coll.Formats.Add(new TResultFormat("HTML File (in List format)", "htm", GCDataFormat.HTML));
            return coll;
        }

    }


    public class TDayEvent : GSCore
    {
        public int nType;
        public int nData;
        public GregorianDateTime Time;
        public int nDst;
        void Set(TDayEvent de)
        {
            nType = de.nType;
            nData = de.nData;
            Time.Set(de.Time);
            nDst = de.nDst;
        }

        public override GSCore GetPropertyValue(string s)
        {
            switch (s)
            {
                case "nType":
                    return new GSNumber(nType);
                case "nData":
                    return new GSNumber(nType);
                case "Time":
                    return Time;
                case "nDst":
                    return new GSNumber(nDst);
                case "tithiName":
                    return new GSString(GCTithi.GetName(nData));
                case "naksatraName":
                    return new GSString(GCNaksatra.GetName(nData));
                case "rasiName":
                    return new GSString(GCRasi.GetName(nData));
                case "groupNameString":
                    return new GSString(GetTypeString(nType));
                case "typeString":
                    return new GSString(GetTypeString(nType, nData));
                case "dstSignature":
                    return new GSString(GCStrings.GetDSTSignature(nDst));
                default:
                    return base.GetPropertyValue(s);
            }
        }

        public static string GetTypeString(int nType)
        {
            switch (nType)
            {
                case CoreEventType.CCTYPE_ASCENDENT:
                    return "Ascendent";
                case CoreEventType.CCTYPE_CONJ:
                    return "Conjunction";
                case CoreEventType.CCTYPE_DATE:
                    return "Date";
                case CoreEventType.CCTYPE_DAY_MUHURTA:
                    return "Muhurta";
                case CoreEventType.CCTYPE_DAY_OF_WEEK:
                    return "Day of Week";
                case CoreEventType.CCTYPE_KALA_START:
                    return "Special interval start";
                case CoreEventType.CCTYPE_KALA_END:
                    return "Special interval end";
                case CoreEventType.CCTYPE_M_RASI:
                    return "Moon rasi";
                case CoreEventType.CCTYPE_M_RISE:
                    return "Moon rise";
                case CoreEventType.CCTYPE_M_SET:
                    return "Moon set";
                case CoreEventType.CCTYPE_NAKS:
                    return "Naksatra";
                case CoreEventType.CCTYPE_S_ARUN:
                    return "Arunodaya";
                case CoreEventType.CCTYPE_S_MIDNIGHT:
                    return "Midnight";
                case CoreEventType.CCTYPE_S_NOON:
                    return "Noon";
                case CoreEventType.CCTYPE_S_RISE:
                    return "Sunrise";
                case CoreEventType.CCTYPE_S_SET:
                    return "Sunset";
                case CoreEventType.CCTYPE_SANK:
                    return "Sankranti";
                case CoreEventType.CCTYPE_TITHI:
                    return "Tithi";
                case CoreEventType.CCTYPE_YOGA:
                    return "Yoga";
                default:
                    return "Unspecified event";
            }

        }

        public static string GetTypeString(int nType, int nData)
        {
            switch (nType)
            {
                case CoreEventType.CCTYPE_ASCENDENT:
                    return "Ascendent " + GCRasi.GetName(nData);
                case CoreEventType.CCTYPE_CONJ:
                    return "Conjunction in " + GCRasi.GetName(nData);
                case CoreEventType.CCTYPE_DATE:
                    return "Date";
                case CoreEventType.CCTYPE_DAY_MUHURTA:
                    return string.Format("{0} Muhurta", GCStrings.GetMuhurtaName(nData));
                case CoreEventType.CCTYPE_DAY_OF_WEEK:
                    return GCCalendar.GetWeekdayName(nData);
                case CoreEventType.CCTYPE_KALA_END:
                    return string.Format("{0} ends", GCStrings.GetKalaName(nData));
                case CoreEventType.CCTYPE_KALA_START:
                    return string.Format("{0} starts", GCStrings.GetKalaName(nData));
                case CoreEventType.CCTYPE_M_RASI:
                    return string.Format("Moon in {0} rasi", GCRasi.GetName(nData));
                case CoreEventType.CCTYPE_M_RISE:
                    return "Moon rise";
                case CoreEventType.CCTYPE_M_SET:
                    return "Moon set";
                case CoreEventType.CCTYPE_NAKS:
                    return string.Format("{0} Naksatra", GCNaksatra.GetName(nData));
                case CoreEventType.CCTYPE_S_ARUN:
                    return "Arunodaya";
                case CoreEventType.CCTYPE_S_MIDNIGHT:
                    return "Midnight";
                case CoreEventType.CCTYPE_S_NOON:
                    return "Noon";
                case CoreEventType.CCTYPE_S_RISE:
                    return "Sunrise";
                case CoreEventType.CCTYPE_S_SET:
                    return "Sunset";
                case CoreEventType.CCTYPE_SANK:
                    return string.Format("{0} Sankranti", GCRasi.GetName(nData));
                case CoreEventType.CCTYPE_TITHI:
                    return string.Format("{0} Tithi", GCTithi.GetName(nData));
                case CoreEventType.CCTYPE_YOGA:
                    return string.Format("{0} Yoga", GCYoga.GetName(nData));
                default:
                    return string.Format("Unspecified event {0} / {1}", nType, nData);
            }

        }

        public string GroupNameString
        {
            get
            {
                return GetTypeString(nType);
            }
        }

        public string TypeString
        {
            get
            {
                return GetTypeString(nType, nData);
            }
        }
    }


    public class TDayEventComparer : Comparer<TDayEvent>
    {
        public int SortType { get; set; }

        public override int Compare(TDayEvent x, TDayEvent y)
        {
            if (SortType == TResultEvents.SORTING_BY_TYPE)
            {
                if (x.nType != y.nType)
                    return x.nType - y.nType;
            }

            double d = x.Time.GetJulianComplete() - y.Time.GetJulianComplete();
            if (d < 0)
                return -1;
            if (d > 0)
                return +1;
            return 0;
        }
    }

}
