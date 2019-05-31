using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public class TResultCalendar: TResultBase
    {
        public static readonly int BEFORE_DAYS = 9;
        public VAISNAVADAY[] m_pData;
        public int m_nCount;
        public int m_PureCount;
        public GCLocation m_Location;
        public GregorianDateTime m_vcStart;
        public int m_vcCount;
        public bool updateCalculationProgress;
        public int nBeg;
        public int nTop;

        public bool EkadasiOnly { get; set; }

        public TResultCalendar()
        {
            EkadasiOnly = false;
            nTop = 0;
            nBeg = 0;
            m_pData = null;
            m_PureCount = 0;
            m_nCount = 0;
            updateCalculationProgress = true;
        }

        public TResultCalendar(GCLocation loc, int nYear) : this()
        {
            CalculateCalendar(loc, new GregorianDateTime(nYear, 1, 1), GregorianDateTime.IsLeapYear(nYear) ? 366 : 365);
        }

        public TResultCalendar(GCLocation loc, int nYear, int nMonth) : this()
        {
            CalculateCalendar(loc, new GregorianDateTime(nYear, nMonth, 1), GregorianDateTime.GetMonthMaxDays(nYear, nMonth));
        }

        public override GSCore GetPropertyValue(string s)
        {
            switch (s)
            {
                case "itemIndexes":
                    GSList list = new GSList();
                    for (int i = 0; i < m_vcCount; i++)
                        list.Add(new GSNumber(i + BEFORE_DAYS));
                    return list;
                case "startDate":
                    return m_vcStart;
                default:
                    return base.GetPropertyValue(s);
            }
        }

        public override GSCore ExecuteMessage(string token, GSCoreCollection args)
        {
            switch(token)
            {
                case "getDay":
                    return GetDay((int)args.getSafe(0).getIntegerValue() - BEFORE_DAYS);
                case "daysToEndweek":
                    return new GSNumber(DAYS_TO_ENDWEEK((int)args.getSafe(0).getIntegerValue()));
                case "daysFromBeginweek":
                    return new GSNumber(DAYS_FROM_BEGINWEEK((int)args.getSafe(0).getIntegerValue()));
                case "dayIndex":
                    return new GSNumber(DAY_INDEX((int)args.getSafe(0).getIntegerValue()));
                default:
                    return base.ExecuteMessage(token, args);
            }
        }

        int DAYS_TO_ENDWEEK(int lastMonthDay)
        {
            return (21 - (lastMonthDay - m_Location.FirstDayOfWeek)) % 7;
        }

        int DAYS_FROM_BEGINWEEK(int firstMonthDay)
        {
            return (firstMonthDay - m_Location.FirstDayOfWeek + 14) % 7;
        }

        int DAY_INDEX(int day)
        {
            return (day + m_Location.FirstDayOfWeek) % 7;
        }

        public static string getDayBkgColorCode(VAISNAVADAY p)
        {
            if (p == null)
                return "white";
            if (p.nFastID == FastType.FAST_EKADASI)
                return "#FFFFBB";
            if (p.nFastID != 0)
                return "#BBFFBB";
            return "white";
        }

        
        /// <summary>
        /// Main function for CALENDAR calculations
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="begDate"></param>
        /// <param name="iCount"></param>
        /// <returns></returns>
        public int CalculateCalendar(GCLocation loc, GregorianDateTime begDate, int iCount)
        {
            int i, weekday;
            int nTotalCount = BEFORE_DAYS + iCount + BEFORE_DAYS;
            GregorianDateTime date;
            GCEarthData earth;
            int lastMasa = 0;
            int lastGYear = 0;
            String tempStr;
            bool bCalcMoon = (GCDisplaySettings.Current.getValue(4) > 0 || GCDisplaySettings.Current.getValue(5) > 0);

            m_nCount = 0;
            m_Location = loc;
            m_vcStart = new GregorianDateTime(begDate);
            m_vcCount = iCount;
            earth = loc.GetEarthData();

            // alokacia pola
            m_pData = new VAISNAVADAY[nTotalCount];

            // inicializacia poctovych premennych
            m_nCount = nTotalCount;
            m_PureCount = iCount;

            date = new GregorianDateTime();
            date.Set(begDate);
            date.shour = 0.0;
            date.TimezoneHours = loc.OffsetUtcHours;
            date.SubtractDays(BEFORE_DAYS);
            date.InitWeekDay();

            weekday = date.dayOfWeek;

            GCFestivalSpecialExecutor exec = new GCFestivalSpecialExecutor(this);

            long utcDayStart = -1;
            long utcDayEnd = -1;

            // 1
            // initialization of days
            for (i = 0; i < nTotalCount; i++)
            {
                m_pData[i] = new VAISNAVADAY();
                m_pData[i].date = new GregorianDateTime(date);
                m_pData[i].date.dayOfWeek = weekday;
                date.NextDay();
                weekday = (weekday + 1) % 7;
                m_pData[i].moonrise.SetValue(-1);
                m_pData[i].moonset.SetValue(-1);

                if (utcDayStart < 0)
                {
                    utcDayStart = Convert.ToInt64(m_pData[i].date.GetJulianComplete() * 86400);
                }
                m_pData[i].UtcDayStart = utcDayStart;
                utcDayStart += 86400;
            }

            for(i = 0; i < nTotalCount; i++)
            {
                m_pData[i].Previous = i > 0 ? m_pData[i - 1] : null;
                m_pData[i].Next = i < nTotalCount - 1 ? m_pData[i + 1] : null;
            }

            TResultCoreEvents recentEvents = null;

            // 2
            // calculating DST data and core events
            foreach (VAISNAVADAY t in m_pData)
            {
                t.BiasMinutes = loc.TimeZone.GetBiasMinutesForDay(t.date);
                if (t.Previous != null)
                {
                    t.DstDayType = (t.BiasMinutes == 0 
                        ? (t.Previous.BiasMinutes == 0 ? DstTypeChange.DstOff : DstTypeChange.DstEnd)
                        : (t.Previous.BiasMinutes == 0 ? DstTypeChange.DstStart : DstTypeChange.DstOn));
                }

                utcDayStart = t.UtcDayStart - (t.Previous != null ? t.Previous.BiasMinutes * 60 : 0);
                utcDayEnd = t.UtcDayStart + 86400 - t.BiasMinutes*60;

                if (recentEvents == null || recentEvents.p_events.Count == 0 || recentEvents.Year != t.date.year)
                    recentEvents = GCCoreAstronomy.GetCoreEventsYear(loc, t.date.year);
                recentEvents.GetCoreEvents(t.coreEvents, utcDayStart, utcDayEnd);


                utcDayStart = utcDayEnd;
            }

            // 3
            if (bCalcMoon)
            {
                foreach (VAISNAVADAY t in m_pData)
                {
                    GCMoonData.CalcMoonTimes(earth, t.date, Convert.ToDouble(t.BiasMinutes/60.0), out t.moonrise, out t.moonset);

                    if (!EkadasiOnly && GCDisplaySettings.Current.getValue(GCDS.CAL_MOON_RISE) != 0 && t.moonrise.hour >= 0)
                    {
                        tempStr = t.Format(GCStrings.Localized("Moonrise {moonRiseTime} ({dstSig})"));
                        t.AddEvent(DisplayPriorities.PRIO_MOON, GCDS.CAL_MOON_RISE, tempStr);
                    }

                    if (!EkadasiOnly && GCDisplaySettings.Current.getValue(GCDS.CAL_MOON_SET) != 0 && t.moonset.hour >= 0)
                    {
                        tempStr = t.Format(GCStrings.Localized("Moonset {moonSetTime} ({dstSig})"));
                        t.AddEvent(DisplayPriorities.PRIO_MOON, GCDS.CAL_MOON_SET, tempStr);
                    }
                }
            }

            // 4
            // init of astro data
            foreach (VAISNAVADAY t in m_pData)
            {
                t.astrodata = new GCAstroData();
                t.astrodata.DayCalc(t.date, earth);

                t.coreEvents.InsertByTime(new TCoreEvent()
                {
                    Time = t.UtcDayStart + (long)t.astrodata.sunArunodaya.TotalSeconds,
                    nType = CoreEventType.CCTYPE_S_ARUN
                });
                t.coreEvents.InsertByTime(new TCoreEvent()
                {
                    Time = t.UtcDayStart + (long)t.astrodata.sunRise.TotalSeconds,
                    nType = CoreEventType.CCTYPE_S_RISE
                });
                t.coreEvents.InsertByTime(new TCoreEvent()
                {
                    Time = t.UtcDayStart + (long)t.astrodata.sunNoon.TotalSeconds,
                    nType = CoreEventType.CCTYPE_S_NOON
                });
                t.coreEvents.InsertByTime(new TCoreEvent()
                {
                    Time = t.UtcDayStart + (long)t.astrodata.sunSet.TotalSeconds,
                    nType = CoreEventType.CCTYPE_S_SET
                });
            }

            // 5
            // init of masa
            foreach (VAISNAVADAY t in m_pData)
            {
                if (t.Previous == null 
                    || t.astrodata.sunRise.Paksa != t.Previous.astrodata.sunRise.Paksa)
                {
                    t.astrodata.MasaCalc(t.date, earth);
                    lastMasa = t.astrodata.Masa;
                    lastGYear = t.astrodata.GaurabdaYear;
                }
                t.astrodata.Masa = lastMasa;
                t.astrodata.GaurabdaYear = lastGYear;

                if (!EkadasiOnly && GCDisplaySettings.Current.getValue(GCDS.CAL_SUN_LONG) != 0)
                {
                    tempStr = string.Format("{0}: {1} (*)", GCStrings.Localized("Sun Longitude")
                        , t.astrodata.sunRise.longitude);
                    t.AddEvent(DisplayPriorities.PRIO_ASTRO, GCDS.CAL_SUN_LONG, tempStr);
                }

                if (!EkadasiOnly && GCDisplaySettings.Current.getValue(GCDS.CAL_MOON_LONG) != 0)
                {
                    tempStr = string.Format("{0}: {1} (*)", GCStrings.Localized("Moon Longitude")
                        , t.astrodata.sunRise.longitudeMoon);
                    t.AddEvent(DisplayPriorities.PRIO_ASTRO, GCDS.CAL_MOON_LONG, tempStr);
                }

                if (!EkadasiOnly && GCDisplaySettings.Current.getValue(GCDS.CAL_AYANAMSHA) != 0)
                {
                    tempStr = string.Format("{0} {1} ({2}) (*)", GCStrings.Localized("Ayanamsha")
                        , t.astrodata.Ayanamsa
                        , GCAyanamsha.GetAyanamsaName(GCAyanamsha.GetAyanamsaType()));
                    t.AddEvent(DisplayPriorities.PRIO_ASTRO, GCDS.CAL_AYANAMSHA, tempStr);
                }

                if (!EkadasiOnly && GCDisplaySettings.Current.getValue(GCDS.CAL_JULIAN) != 0)
                {
                    tempStr = string.Format("{0} {1} (*)", GCStrings.Localized("Julian Time")
                        , t.astrodata.JulianDay);
                    t.AddEvent(DisplayPriorities.PRIO_ASTRO, GCDS.CAL_JULIAN, tempStr);
                }
            }


            if (!EkadasiOnly && GCDisplaySettings.Current.getValue(GCDS.CAL_MASA_CHANGE) != 0)
            {
                String str;

                foreach (VAISNAVADAY t in m_pData)
                {
                    if (t.Previous != null && t.Previous.astrodata.Masa != t.astrodata.Masa)
                    {
                        str = t.Format(GCStrings.Localized("First day of {masaName} Masa"));
                        t.AddEvent(DisplayPriorities.PRIO_MASA_CHANGE, GCDS.CAL_MASA_CHANGE, str);
                    }

                    if (t.Next != null && t.Next.astrodata.Masa != t.astrodata.Masa)
                    {
                        str = t.Format(GCStrings.Localized("Last day of {masaName} Masa"));
                        t.AddEvent(DisplayPriorities.PRIO_MASA_CHANGE, GCDS.CAL_MASA_CHANGE, str);
                    }
                }
            }

            if (!EkadasiOnly && GCDisplaySettings.Current.getValue(GCDS.CAL_DST_CHANGE) != 0)
            {
                foreach (VAISNAVADAY t in m_pData)
                {
                    if (t.Previous != null && t.Previous.BiasMinutes == 0 && t.BiasMinutes != 0)
                        t.AddEvent(DisplayPriorities.PRIO_DST_CHANGE, GCDS.CAL_DST_CHANGE, GCStrings.Localized("First day of Daylight Saving Time"));
                    else if (t.Next != null && t.BiasMinutes != 0 && t.Next.BiasMinutes == 0)
                        t.AddEvent(DisplayPriorities.PRIO_DST_CHANGE, GCDS.CAL_DST_CHANGE, GCStrings.Localized("Last day of Daylight Saving Time"));
                }
            }

            // 6
            // init of mahadvadasis
            foreach (VAISNAVADAY t in m_pData)
            {
                t.MahadvadasiCalc(earth);
            }

            // 
            // init for Ekadasis
            foreach (VAISNAVADAY t in m_pData)
            {
                if (t.Previous == null)
                    continue;
                t.EkadasiCalc(earth);
                if (t.Previous.nFastID == FastType.FAST_EKADASI)
                {
                    t.CalculateEParana(earth);
                }
            }

            if (EkadasiOnly)
                return 1;

            // init ksaya data
            // init of second day of vriddhi
            CalculateKsayaVriddhiTithis();

            //
            // calculate sankrantis
            CalculateSankrantis();

            // 7
            // init of festivals
            AddMatchedFestivals(exec);

            //
            // apply daylight saving time
            ApplyDaylightSavingHours();

            //
            // resolve festivals fasting
            for (i = BEFORE_DAYS; i < m_PureCount + BEFORE_DAYS; i++)
            {
                ResolveFestivalsFasting(i);

                if (GCDisplaySettings.Current.getValue(GCDS.CAL_SUN_RISE) != 0)
                {
                    tempStr = string.Format("{0}-{1}-{2}  {3} - {4} - {5} ({6})",
                        GCStrings.Localized("Sunrise"),
                        GCStrings.Localized("Noon"),
                        GCStrings.Localized("Sunset"),
                        m_pData[i].astrodata.sunRise.ToShortTimeString(),
                        m_pData[i].astrodata.sunNoon.ToShortTimeString(),
                        m_pData[i].astrodata.sunSet.ToShortTimeString(),
                        GCStrings.GetDSTSignature(m_pData[i].BiasMinutes));
                    m_pData[i].AddEvent(DisplayPriorities.PRIO_SUN, GCDS.CAL_SUN_RISE, tempStr);
                }

                if (GCDisplaySettings.Current.getValue(GCDS.CAL_SUN_SANDHYA) != 0)
                {
                    tempStr = string.Format("{0}: {1} | {2} | {3}   ({4})",
                        GCStrings.Localized("Sandhyas"),
                        m_pData[i].astrodata.sunRise.ShortSandhyaString(),
                        m_pData[i].astrodata.sunNoon.ShortSandhyaString(),
                        m_pData[i].astrodata.sunSet.ShortSandhyaString(),
                        GCStrings.GetDSTSignature(m_pData[i].BiasMinutes));
                    m_pData[i].AddEvent(DisplayPriorities.PRIO_SUN, GCDS.CAL_SUN_SANDHYA, tempStr);
                }

                if (GCDisplaySettings.Current.getValue(GCDS.CAL_BRAHMA_MUHURTA) != 0)
                {
                    tempStr = string.Format("{0}: {1}   ({2})",
                        GCStrings.Localized("Brahma Muhurta"),
                        m_pData[i].astrodata.sunRise.ShortMuhurtaString(-2),
                        GCStrings.GetDSTSignature(m_pData[i].BiasMinutes));
                    m_pData[i].AddEvent(DisplayPriorities.PRIO_SUN, GCDS.CAL_BRAHMA_MUHURTA, tempStr);
                }
            }

            if (GCDisplaySettings.Current.getValue(GCDS.CAL_COREEVENTS) != 0)
            {
                int number = 1;
                foreach (VAISNAVADAY today in m_pData)
                { 
                    foreach (TCoreEvent tde in today.coreEvents)
                    { 
                        m_pData[i].AddEvent(DisplayPriorities.PRIO_ASTRO + number, GCDS.CAL_COREEVENTS,
                            tde.TypeString + "   " + today.GetCoreEventTime(tde));
                        number++;
                    }
                }
            }


            // sorting day events according priority
            VAISNAVAEVENTComparer vec = new VAISNAVAEVENTComparer();
            foreach (VAISNAVADAY t in m_pData)
            {
                t.dayEvents.Sort(vec);
            }


            return 1;

        }

        private void ApplyDaylightSavingHours()
        {
            for (int i = BEFORE_DAYS; i < m_PureCount + BEFORE_DAYS; i++)
            {
                VAISNAVADAY today = m_pData[i];
                if (today.eparana_time1 != null)
                    today.eparana_time1.ApplyDstType(today.UtcDayStart, today.DstDayType);

                if (today.eparana_time2 != null)
                    today.eparana_time2.ApplyDstType(today.UtcDayStart, today.DstDayType);

                if (today.astrodata.sunRise.longitude > 0.0)
                {
                    today.astrodata.sunRise.AddMinutes(today.BiasMinutes);
                    today.astrodata.sunSet.AddMinutes(today.BiasMinutes);
                    today.astrodata.sunNoon.AddMinutes(today.BiasMinutes);
                    today.astrodata.sunArunodaya.AddMinutes(today.BiasMinutes);
                }

                foreach(TCoreEvent core in today.coreEvents)
                {
                    core.ApplyDstType(today.UtcDayStart, today.DstDayType);
                }
            }
        }

        private void CalculateKsayaVriddhiTithis()
        {
            GCEarthData earth = m_Location.GetEarthData();

            foreach (VAISNAVADAY t in m_pData)
            {
                VAISNAVADAY s = t.Previous;

                if (s == null)
                    continue;

                if (t.astrodata.sunRise.Tithi == s.astrodata.sunRise.Tithi)
                {
                    t.vriddhiDayNo = 2;
                    if (GCDisplaySettings.Current.getValue(GCDS.CAL_VRDDHI) != 0)
                        t.AddEvent(DisplayPriorities.PRIO_KSAYA, GCDS.CAL_VRDDHI, GCStrings.getString(90));
                }
                else if (t.astrodata.sunRise.Tithi != GCTithi.NEXT_TITHI(s.astrodata.sunRise.Tithi))
                {
                    s.ksayaTithi = GCTithi.NEXT_TITHI(s.astrodata.sunRise.Tithi);
                    s.ksayaMasa = (s.ksayaTithi == 0 ? t.astrodata.Masa : s.astrodata.Masa);
                    int idx = 0;
                    String str;

                    List<VAISNAVADAY.CoreEventFindRec> tithiTimes = t.GetRecentCoreTimes(CoreEventType.CCTYPE_S_RISE, CoreEventType.CCTYPE_TITHI, 2);

                    if (tithiTimes.Count == 2)
                    {
                        str = t.Format(GCStrings.Localized("Kshaya tithi: {prevTithiName} — {0} to {1} ({dstSig})"),
                            tithiTimes[0].dateTimeOfEvent.Format("{day} {monthAbr} {hour}:{minRound}"),
                            tithiTimes[1].dateTimeOfEvent.Format("{day} {monthAbr} {hour}:{minRound}"));

                        t.AddEvent(DisplayPriorities.PRIO_KSAYA, GCDS.CAL_KSAYA, str);
                    }
                }
            }
        }

        protected void CalculateSankrantis()
        {
            VAISNAVADAY targetDay = null;
            foreach(VAISNAVADAY today in m_pData)
            {
                targetDay = null;
                int n = 0;
                foreach (TCoreEvent ce in today.coreEvents)
                {
                    switch(ce.nType)
                    {
                        case CoreEventType.CCTYPE_SANK:
                            switch(GCSankranti.GetSankrantiType())
                            {
                                case 0: targetDay = today; break;
                                case 1: targetDay = (n >= 1 ? today : today.Previous); break;
                                case 2: targetDay = (n >= 2 ? today.Next : today); break;
                                case 3: targetDay = (n >= 3 ? today.Next : today); break;
                            }
                            if (targetDay != null)
                            {
                                targetDay.sankranti_day = today.GetGregorianDateTime(ce);
                                targetDay.sankranti_zodiac = ce.nData;
                            }
                            break;
                        case CoreEventType.CCTYPE_S_RISE: n = 1; break;
                        case CoreEventType.CCTYPE_S_NOON: n = 2; break;
                        case CoreEventType.CCTYPE_S_SET:  n = 3; break;
                    }

                    if (targetDay != null)
                    {
                        string str = targetDay.Format(GCStrings.Localized("  {sankranti.rasiName} Sankranti (Sun enters {sankranti.rasiNameEn} on {sankranti.day} {sankranti.monthAbr}, {sankranti.hour}:{sankranti.minRound}) ({dstSig})"));
                        VAISNAVAEVENT dc = targetDay.AddEvent(DisplayPriorities.PRIO_SANKRANTI, GCDS.CAL_SANKRANTI, str);
                        dc.spec = "sankranti";
                        break;
                    }
                }
            }

        }

        public int AddMatchedFestivals(GCFestivalSpecialExecutor exec)
        {
            int currFestTop = 0;

            for (int i = BEFORE_DAYS; i < m_PureCount + BEFORE_DAYS - 1; i++)
            {
                exec.CurrentIndex = i;
                foreach (GCFestivalBook book in GCFestivalBookCollection.Books)
                {
                    if (!book.Visible)
                        continue;

                    foreach (GCFestivalBase fb in book.Festivals)
                    {
                        if (fb.nReserved == 1 && fb.nVisible > 0 && fb.IsFestivalDay(exec))
                        {
                            currFestTop = AddEventToDay(exec, 0, currFestTop, fb);
                        }
                    }
                }
            }

            return 1;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="exec"></param>
        /// <param name="offsetWithToday">If value of this parameter is 0, then current day is processed,
        /// -1 is for yesterday, +1 is for tomorrow</param>
        /// <param name="currFestTop"></param>
        /// <param name="fb"></param>
        /// <returns></returns>
        private int AddEventToDay(GCFestivalSpecialExecutor exec, int offsetWithToday, int currFestTop, GCFestivalBase fb)
        {
            VAISNAVADAY t = exec.day(offsetWithToday);
            VAISNAVAEVENT md = t.AddEvent(DisplayPriorities.PRIO_FESTIVALS_0 + fb.BookID * 100 + currFestTop, 
                GCDS.CAL_FEST_0 + fb.BookID, fb.Text);
            currFestTop += 5;
            if (fb.FastID > 0)
            {
                md.fasttype = fb.FastID;
                md.fastsubject = fb.FastingSubject;
            }

            if (GCDisplaySettings.Current.getValue(51) != 2 && fb.StartYear > -7000)
            {
                String ss1;
                int years = t.astrodata.GaurabdaYear - (fb.StartYear - 1496);
                string appx = "th";
                if (years % 10 == 1) appx = "st";
                else if (years % 10 == 2) appx = "nd";
                else if (years % 10 == 3) appx = "rd";
                if (GCDisplaySettings.Current.getValue(51) == 0)
                {
                    ss1 = string.Format("{0} ({1}{2} anniversary)", md.text, years, appx);
                }
                else
                {
                    ss1 = string.Format("{0} ({1}{2})", md.text, years, appx);
                }
                md.text = ss1;
            }

            if (fb.EventsCount > 0)
            {
                foreach (GCFestivalBase re in fb.Events)
                {
                    if (re is GCFestivalRelated)
                    {
                        GCFestivalRelated related = re as GCFestivalRelated;
                        AddEventToDay(exec, fb.DayOffset + related.DayOffset, 0, related);
                    }
                }
            }

            return currFestTop;
        }

        public VAISNAVADAY GetDay(int nIndex)
        {
            int nReturn = nIndex + BEFORE_DAYS;

            if (nReturn >= m_nCount)
                return null;

            return m_pData[nReturn];
        }

        public int FindDate(GregorianDateTime vc)
        {
            int i;
            for (i = BEFORE_DAYS; i < m_nCount; i++)
            {
                if ((m_pData[i].date.day == vc.day) && (m_pData[i].date.month == vc.month) && (m_pData[i].date.year == vc.year))
                    return (i - BEFORE_DAYS);
            }

            return -1;
        }

        public void ResolveFestivalsFasting(int nIndex)
        {
            VAISNAVADAY s = m_pData[nIndex - 1];
            VAISNAVADAY t = m_pData[nIndex];
            VAISNAVADAY u = m_pData[nIndex + 1];

            String str;
            String subject = string.Empty;
            int fastForDay = 0;
            int fastForEvent = 0;
            string ch;
            VAISNAVAEVENT md;

            if (t.nMahadvadasiID != MahadvadasiType.EV_NULL)
            {
                str = string.Format(GCStrings.Localized("Fasting for {0}"), t.ekadasi_vrata_name);
                t.AddEvent(DisplayPriorities.PRIO_EKADASI, GCDS.CAL_EKADASI_PARANA, str);
            }

            ch = GCEkadasi.GetMahadvadasiName((int)t.nMahadvadasiID);
            if (ch != null)
            {
                t.AddEvent(DisplayPriorities.PRIO_MAHADVADASI, GCDS.CAL_EKADASI_PARANA, ch);
            }

            if (t.ekadasi_parana)
            {
                str = t.GetTextEP();
                t.AddEvent(DisplayPriorities.PRIO_EKADASI_PARANA, GCDS.CAL_EKADASI_PARANA, str);
            }

            for (int h = 0; h < t.dayEvents.Count(); h++)
            {
                md = t.dayEvents[h];
                fastForEvent = FastType.FAST_NULL;
                if (md.fasttype != FastType.FAST_NULL)
                {
                    fastForEvent = md.fasttype;
                    subject = md.fastsubject;
                }

                if (fastForEvent != FastType.FAST_NULL)
                {
                    if (s.nFastID == FastType.FAST_EKADASI)
                    {
                        if (GCDisplaySettings.Current.getValue(42) == 0)
                        {
                            str = string.Format(GCStrings.Localized("(Fast today for {0})"), subject);
                            s.AddEvent(DisplayPriorities.PRIO_FASTING, GCDS.DISP_ALWAYS, str);
                            t.AddEvent(md.prio + 1, md.dispItem, GCStrings.Localized("(Fasting is done yesterday)"));
                            //"(Fasting is done yesterday)"
                        }
                        else
                        {
                            str = string.Format(GCStrings.Localized("(Fast till noon for {0}, with feast tomorrow)"), subject);
                            s.AddEvent(DisplayPriorities.PRIO_FASTING, GCDS.DISP_ALWAYS, str);
                            t.AddEvent(md.prio + 1, md.dispItem, GCStrings.Localized("(Fasting is done yesterday, today is feast)"));
                            //"(Fasting is done yesterday, today is feast)";
                        }
                    }
                    else if (t.nFastID == FastType.FAST_EKADASI)
                    {
                        if (GCDisplaySettings.Current.getValue(42) != 0)
                            t.AddEvent(md.prio + 1, md.dispItem, GCStrings.Localized("(Fasting till noon, with feast tomorrow)"));
                        //"(Fasting till noon, with feast tomorrow)";
                        else
                            t.AddEvent(md.prio + 1, md.dispItem, GCStrings.Localized("(Fast today)"));
                        //"(Fast today)"
                    }
                    else
                    {
                        /* OLD STYLE FASTING
                        if (GCDisplaySettings.Current.getValue(42) == 0)
                        {
                            if (nftype > 1)
                                nftype = 7;
                            else nftype = 0;
                        }*/
                        if (fastForEvent != FastType.FAST_NULL)
                        {
                            t.AddEvent(md.prio + 1, md.dispItem,
                                GCStrings.GetFastingName(fastForEvent));
                        }
                    }
                }
                if (fastForDay < fastForEvent)
                    fastForDay = fastForEvent;
            }

            if (fastForDay != FastType.FAST_NULL)
            {
                if (s.nFastID == FastType.FAST_EKADASI)
                {
                    t.nFeasting = FeastType.FEAST_TODAY_FAST_YESTERDAY;
                    s.nFeasting = FeastType.FEAST_TOMMOROW_FAST_TODAY;
                }
                else if (t.nFastID == FastType.FAST_EKADASI)
                {
                    u.nFeasting = FeastType.FEAST_TODAY_FAST_YESTERDAY;
                    t.nFeasting = FeastType.FEAST_TOMMOROW_FAST_TODAY;
                }
                else
                {
                    t.nFastID = fastForDay;
                }
            }

        }

        public override string formatText(string df)
        {
            GSScript script = new GSScript();
            switch (df)
            {
                case GCDataFormat.PlainText:
                    script.readTextTemplate(Properties.Resources.TplCalendarPlain);
                    break;
                case GCDataFormat.Rtf:
                    script.readTextTemplate(Properties.Resources.TplCalendarRtf);
                    break;
                case GCDataFormat.HTML:
                    script.readTextTemplate(Properties.Resources.TplCalendarHtml);
                    break;
                case GCDataFormat.XML:
                    script.readTextTemplate(Properties.Resources.TplCalendarXml);
                    break;
                case GCDataFormat.CSV:
                    script.readTextTemplate(Properties.Resources.TplCalendarCsv);
                    break;
                case GCDataFormat.ICAL:
                    script.readTextTemplate(Properties.Resources.TplCalendarICAL);
                    break;
                case GCDataFormat.VCAL:
                    script.readTextTemplate(Properties.Resources.TplCalendarVCAL);
                    break;
                case "htmlTable":
                    script.readTextTemplate(Properties.Resources.TplCalendarHtmlTable);
                    break;
                case "htmlSadhana":
                    script.readTextTemplate(Properties.Resources.TplCalendarSadhana);
                    break;
                default:
                    break;
            }


            GSExecutor engine = new GSExecutor();
            engine.SetVariable("calendar", this);
            engine.SetVariable("location", this.m_Location);
            engine.SetVariable("app", GCUserInterface.Shared);
            engine.ExecuteElement(script);

            return engine.getOutput();
        }

        public override TResultFormatCollection getFormats()
        {
            TResultFormatCollection coll = base.getFormats();

            coll.ResultName = "Calendar";
            coll.Formats.Add(new TResultFormat("Text File", "txt", GCDataFormat.PlainText));
            coll.Formats.Add(new TResultFormat("Rich Text File", "rtf", GCDataFormat.Rtf));
            coll.Formats.Add(new TResultFormat("XML File", "xml", GCDataFormat.XML));
            coll.Formats.Add(new TResultFormat("iCalendar File", "ics", GCDataFormat.ICAL));
            coll.Formats.Add(new TResultFormat("vCalendar File", "vcs", GCDataFormat.VCAL));
            coll.Formats.Add(new TResultFormat("Comma Separated Values", "csv", GCDataFormat.CSV));
            coll.Formats.Add(new TResultFormat("HTML File (in Table format)", "htm", "htmlTable"));
            coll.Formats.Add(new TResultFormat("HTML File (in List format)", "htm", GCDataFormat.HTML));
            coll.Formats.Add(new TResultFormat("HTML format daily sadhana", "htm", "htmlSadhana"));
            return coll;
        }


        public static string getPlainDayTemplate()
        {
            return Properties.Resources.TplCalendarDayPlain;
        }
    }
}
