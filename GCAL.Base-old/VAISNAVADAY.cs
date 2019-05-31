using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public class VAISNAVAEVENT: GSCore
    {
        public int prio;
        public int dispItem;
        public string text;
        public int fasttype;
        public string fastsubject;
        public string spec = null;

        public override GSCore GetPropertyValue(string s)
        {
            switch (s)
            {
                case "prio": return new GSNumber() { IntegerValue = prio };
                case "dispItem": return new GSNumber() { IntegerValue = dispItem };
                case "fastType": return new GSNumber() { IntegerValue = fasttype };
                case "text": return new GSString() { Value = text };
                case "htmlText": return new GSString() { 
                    Value = GetHtmlText() 
                };
                case "fastSubject": return new GSString() { Value = text };
                case "spec": return new GSString() { Value = (spec != null ? spec : string.Empty) };
                case "isSpec": return new GSBoolean() { Value = (spec != null) };
                case "specTextColor": return new GSString() { Value = "blue" };
                default: break;
            }
            return base.GetPropertyValue(s);
        }

        public string GetHtmlText()
        {
            if (dispItem == GCDS.CAL_SUN_RISE || dispItem == GCDS.CAL_SUN_SANDHYA ||
                dispItem == GCDS.CAL_BRAHMA_MUHURTA)
                return "&#9724; " + text;
            else if (dispItem == GCDS.CAL_COREEVENTS)
                return "&#9723; " + text;
            else
                return text;
        }

    }

    public class VAISNAVADAY: GSCore
    {
        public struct CoreEventFindRec
        {
            public VAISNAVADAY day;
            public TCoreEvent coreEvent;
            public GregorianDateTime dateTimeOfEvent;
        }

        public VAISNAVADAY Previous;
        public VAISNAVADAY Next;

        // date
        public GregorianDateTime date;
        // moon times
        public GCHourTime moonrise;
        public GCHourTime moonset;
        // astronomical data from astro-sub-layer
        public GCAstroData astrodata;

        public TCoreEventCollection coreEvents;

        public int BiasMinutes;
        public DstTypeChange DstDayType = DstTypeChange.DstOff;
        public long UtcDayStart;

        //
        // day events and fast
        public List<VAISNAVAEVENT> dayEvents;
        public int nFeasting;
        public int nFastID;

        //
        // Ekadasi data
        //
        public int nMahadvadasiID;
        public String ekadasi_vrata_name;
        public bool ekadasi_parana;
        public TCoreEvent eparana_time1;
        public TCoreEvent eparana_time2;

        //
        // Sankranti data
        public int sankranti_zodiac;
        public GregorianDateTime sankranti_day;

        // Ksaya and Vridhi data
        //
        public int ksayaTithi = -1;
        public int ksayaMasa = -1;
        public int vriddhiDayNo = 1;


        public VAISNAVADAY()
        {
            nFastID = FastType.FAST_NULL;
            nMahadvadasiID = MahadvadasiType.EV_NULL;
            ekadasi_parana = false;
            ekadasi_vrata_name = "";
            eparana_time1 = eparana_time2 = null;
            sankranti_zodiac = -1;
            BiasMinutes = 0;
            moonrise.SetValue(0);
            moonset.SetValue(0);
            UtcDayStart = -1;
            dayEvents = new List<VAISNAVAEVENT>();
            coreEvents = new TCoreEventCollection();
        }

        public override string ToString()
        {
            return string.Format("{0}, Fast:{1}, Tithi:{2}, Masa:{3}", date.ToString(), GCStrings.GetFastingName(this.nFastID), GCTithi.GetName(this.astrodata.sunRise.Tithi),
                GCMasa.GetName(astrodata.Masa));
        }

        public override GSCore GetPropertyValue(string Token)
        {
            if (Token.Equals("date"))
            {
                return date;
            }
            else if (Token.Equals("astro"))
            {
                return astrodata;
            }
            else if (Token.Equals("dateHumanName"))
            {
                return new GSString(GregorianDateTime.GetDateTextWithTodayExt(date));
            }
            else if (Token.Equals("nDST"))
                return new GSNumber(BiasMinutes);
            else if (Token.Equals("events"))
            {
                GSList list = new GSList();
                list.Parts.AddRange(dayEvents);
                return list;
            }
            else if (Token.Equals("visibleEvents"))
            {
                GSList list = new GSList();
                list.Parts.AddRange(VisibleEvents);
                return list;
            }
            else if (Token.Equals("htmlDayColor"))
            {
                return new GSString(TResultCalendar.getDayBkgColorCode(this));
            }
            else if (Token.Equals("dstSignature"))
            {
                return new GSString(GCStrings.GetDSTSignature(BiasMinutes));
            }
            else if (Token.Equals("tithiNameExt"))
            {
                return new GSString(GetFullTithiName());
            }
            else if (Token.Equals("isWeekend"))
            {
                return new GSBoolean(date.dayOfWeek == 6 || date.dayOfWeek == 0);
            }
            else if (Token.Equals("fastType"))
                return new GSNumber(nFastID);
            else if (Token.Equals("fastTypeMark"))
                return new GSString(nFastID != 0 ? "*" : " ");
            else if (Token.Equals("ekadasiParana"))
                return new GSBoolean(ekadasi_parana);
            else if (Token.Equals("ekadasiParanaStart"))
            {
                return GetGregorianDateTime(eparana_time1);
            }
            else if (Token.Equals("ekadasiParanaEnd"))
            {
                return GetGregorianDateTime(eparana_time2);
            }
            else if (Token.Equals("hasParanaStart"))
                return new GSBoolean(eparana_time1 != null);
            else if (Token.Equals("hasParanaEnd"))
                return new GSBoolean(eparana_time2 != null);
            else if (Token.Equals("sankrantiZodiac"))
                return new GSNumber(sankranti_zodiac);
            else if (Token.Equals("sankrantiDateTime"))
                return sankranti_day;
            else if (Token.Equals("ksayaTithi"))
                return new GSNumber(ksayaTithi);
            else if (Token.Equals("ksayaMasa"))
                return new GSNumber(ksayaMasa);
            else
            {
                return base.GetPropertyValue(Token);
            }
        }

        public GregorianDateTime GetGregorianDateTime(TCoreEvent ce)
        {
            GregorianDateTime gdt = new GregorianDateTime(date);
            gdt.shour = (ce.GetDstTime(BiasMinutes * 60) - UtcDayStart) / 86400.0;
            return gdt;
        }

        public string GetCoreEventTime(TCoreEvent tce)
        {
            return GregorianDateTime.TimeSpanToLongString(tce.GetDstTime(this.BiasMinutes * 60) - this.UtcDayStart) 
                + " ("
                + GCStrings.GetDSTSignature(tce.nDst * this.BiasMinutes) 
                + ")";
        }

        public List<VAISNAVAEVENT> VisibleEvents
        {
            get
            {
                List<VAISNAVAEVENT> ve = new List<VAISNAVAEVENT>();
                foreach (VAISNAVAEVENT ed in dayEvents)
                {
                    int disp = ed.dispItem;
                    if (ed.dispItem != 0 && (disp == -1 || GCDisplaySettings.Current.getValue(disp) != 0))
                    {
                        ve.Add(ed);
                    }
                }

                return ve;
            }
        }

        public int EkadasiCalc(GCEarthData earth)
        {
            VAISNAVADAY t = this;

            if (t.Previous == null || t.Next == null)
                return 0;

            VAISNAVADAY s = t.Previous;
            VAISNAVADAY u = t.Next;

            if (GCTithi.TITHI_EKADASI(t.astrodata.sunRise.Tithi))
            {
                // if TAT < 11 then NOT_EKADASI
                if (GCTithi.TITHI_LESS_EKADASI(t.astrodata.sunArunodaya.Tithi))
                {
                    t.nMahadvadasiID = MahadvadasiType.EV_NULL;
                    t.ekadasi_vrata_name = "";
                    t.nFastID = FastType.FAST_NULL;
                }
                else
                {
                    // else ak MD13 then MHD1 and/or 3
                    if (GCTithi.TITHI_EKADASI(s.astrodata.sunRise.Tithi) && GCTithi.TITHI_EKADASI(s.astrodata.sunArunodaya.Tithi))
                    {
                        if (GCTithi.TITHI_TRAYODASI(u.astrodata.sunRise.Tithi))
                        {
                            t.nMahadvadasiID = MahadvadasiType.EV_UNMILANI_TRISPRSA;
                            t.ekadasi_vrata_name = GCEkadasi.GetEkadasiName(t.astrodata.Masa, t.astrodata.sunRise.Paksa);
                            t.nFastID = FastType.FAST_EKADASI;
                        }
                        else
                        {
                            t.nMahadvadasiID = MahadvadasiType.EV_UNMILANI;
                            t.ekadasi_vrata_name = GCEkadasi.GetEkadasiName(t.astrodata.Masa, t.astrodata.sunRise.Paksa);
                            t.nFastID = FastType.FAST_EKADASI;
                        }
                    }
                    else
                    {
                        if (GCTithi.TITHI_TRAYODASI(u.astrodata.sunRise.Tithi))
                        {
                            t.nMahadvadasiID = MahadvadasiType.EV_TRISPRSA;
                            t.ekadasi_vrata_name = GCEkadasi.GetEkadasiName(t.astrodata.Masa, t.astrodata.sunRise.Paksa);
                            t.nFastID = FastType.FAST_EKADASI;
                        }
                        else
                        {
                            // else ak U je MAHADVADASI then NOT_EKADASI
                            if (GCTithi.TITHI_EKADASI(u.astrodata.sunRise.Tithi) || (u.nMahadvadasiID >= MahadvadasiType.EV_SUDDHA))
                            {
                                t.nMahadvadasiID = MahadvadasiType.EV_NULL;
                                t.ekadasi_vrata_name = "";
                                t.nFastID = FastType.FAST_NULL;
                            }
                            else if (u.nMahadvadasiID == MahadvadasiType.EV_NULL)
                            {
                                // else suddha ekadasi
                                t.nMahadvadasiID = MahadvadasiType.EV_SUDDHA;
                                t.ekadasi_vrata_name = GCEkadasi.GetEkadasiName(t.astrodata.Masa, t.astrodata.sunRise.Paksa);
                                t.nFastID = FastType.FAST_EKADASI;
                            }
                        }
                    }
                }
            }
            // test for break fast


            return 1;
        }

        /*
         * Function before is writen accoring this algorithms:


            1. Normal - fasting day has ekadasi at sunrise and dvadasi at next sunrise.

            2. Viddha - fasting day has dvadasi at sunrise and trayodasi at next
            sunrise, and it is not a naksatra mahadvadasi

            3. Unmilani - fasting day has ekadasi at both sunrises

            4. Vyanjuli - fasting day has dvadasi at both sunrises, and it is not a
            naksatra mahadvadasi

            5. Trisprsa - fasting day has ekadasi at sunrise and trayodasi at next
            sunrise.

            6. Jayanti/Vijaya - fasting day has gaura dvadasi and specified naksatra at
            sunrise and same naksatra at next sunrise

            7. Jaya/Papanasini - fasting day has gaura dvadasi and specified naksatra at
            sunrise and same naksatra at next sunrise

            ==============================================
            Case 1 Normal (no change)

            If dvadasi tithi ends before 1/3 of daylight
               then PARANA END = TIME OF END OF TITHI
            but if dvadasi TITHI ends after 1/3 of daylight
               then PARANA END = TIME OF 1/3 OF DAYLIGHT

            if 1/4 of dvadasi tithi is before sunrise
               then PARANA BEGIN is sunrise time
            but if 1/4 of dvadasi tithi is after sunrise
               then PARANA BEGIN is time of 1/4 of dvadasi tithi

            if PARANA BEGIN is before PARANA END
               then we will write "BREAK FAST FROM xx TO yy
            but if PARANA BEGIN is after PARANA END
               then we will write "BREAK FAST AFTER xx"

            ==============================================
            Case 2 Viddha

            If trayodasi tithi ends before 1/3 of daylight
               then PARANA END = TIME OF END OF TITHI
            but if trayodasi TITHI ends after 1/3 of daylight
               then PARANA END = TIME OF 1/3 OF DAYLIGHT

            PARANA BEGIN is sunrise time

            we will write "BREAK FAST FROM xx TO yy

            ==============================================
            Case 3 Unmilani

            PARANA END = TIME OF 1/3 OF DAYLIGHT

            PARANA BEGIN is end of Ekadasi tithi

            if PARANA BEGIN is before PARANA END
               then we will write "BREAK FAST FROM xx TO yy
            but if PARANA BEGIN is after PARANA END
               then we will write "BREAK FAST AFTER xx"

            ==============================================
            Case 4 Vyanjuli

            PARANA BEGIN = Sunrise

            PARANA END is end of Dvadasi tithi

            we will write "BREAK FAST FROM xx TO yy

            ==============================================
            Case 5 Trisprsa

            PARANA BEGIN = Sunrise

            PARANA END = 1/3 of daylight hours

            we will write "BREAK FAST FROM xx TO yy

            ==============================================
            Case 6 Jayanti/Vijaya

            PARANA BEGIN = Sunrise

            PARANA END1 = end of dvadasi tithi or sunrise, whichever is later
            PARANA END2 = end of naksatra

            PARANA END is earlier of END1 and END2

            we will write "BREAK FAST FROM xx TO yy

            ==============================================
            Case 7 Jaya/Papanasini

            PARANA BEGIN = end of naksatra

            PARANA END = 1/3 of Daylight hours

            if PARANA BEGIN is before PARANA END
               then we will write "BREAK FAST FROM xx TO yy
            but if PARANA BEGIN is after PARANA END
               then we will write "BREAK FAST AFTER xx"


 * */
        public int CalculateEParana(GCEarthData earth)
        {
            VAISNAVADAY t = this;
            if (t.Previous == null)
                return 0;

            t.nMahadvadasiID = MahadvadasiType.EV_NULL;
            t.ekadasi_parana = true;
            t.nFastID = FastType.FAST_NULL;

            TCoreEvent naksEnd;
            TCoreEvent parBeg = null;
            TCoreEvent parEnd = null;
            TCoreEvent tithiStart = null;
            TCoreEvent tithiEnd = null;
            TCoreEvent sunRise = t.FindCoreEvent(CoreEventType.CCTYPE_S_RISE);
            TCoreEvent sunSet = t.FindCoreEvent(CoreEventType.CCTYPE_S_SET);
            if (sunRise == null || sunSet == null)
            {
                GCLog.Write("Cannot find sunrise of sunset for day " + t.date.ToString());
                return 0;
            }

            TCoreEvent third_day = new TCoreEvent() {
                nData = sunRise.nData,
                nType = CoreEventType.CCTYPE_THIRD_OF_DAY,
                nDst = sunRise.nDst,
                Time = (sunSet.Time - sunRise.Time) / 3 + sunRise.Time
            };

            List<CoreEventFindRec> tempTimes = GetRecentCoreTimes(CoreEventType.CCTYPE_S_RISE, CoreEventType.CCTYPE_TITHI, 1);
            if (tempTimes.Count != 1)
            {
                GCLog.Write("Start or End of tithi was not found for date " + t.ToString());
                return 0;
            }
            else
            {
                tithiStart = tempTimes[0].coreEvent;
            }

            tempTimes = GetNextCoreTimes(CoreEventType.CCTYPE_S_RISE, CoreEventType.CCTYPE_TITHI, 1);
            if (tempTimes.Count != 1)
            {
                GCLog.Write("End of tithi was not found for date " + t.ToString());
                tempTimes = GetNextCoreTimes(CoreEventType.CCTYPE_S_RISE, CoreEventType.CCTYPE_TITHI, 1);
                return 0;
            }
            else
            {
                tithiEnd = new TCoreEvent(tempTimes[0].coreEvent) { nType = CoreEventType.CCTYPE_TITHI_END };
                tithiEnd.ApplyDstType(UtcDayStart, DstDayType);
            }

            if (Previous == null)
                return 0;

            tempTimes = Previous.GetNextCoreTimes(CoreEventType.CCTYPE_S_RISE, CoreEventType.CCTYPE_NAKS, 1);
            if (tempTimes.Count != 1)
            {
                GCLog.Write("End of naksatra was not found for date " + t.ToString());
                return 0;
            }
            else
            {
                naksEnd = new TCoreEvent(tempTimes[0].coreEvent) { nType = CoreEventType.CCTYPE_NAKS_END };
                naksEnd.ApplyDstType(UtcDayStart, DstDayType);

            }

            TCoreEvent tithi_quart = new TCoreEvent()
            {
                nType = CoreEventType.CCTYPE_TITHI_QUARTER,
                nData = tithiStart.nData,
                Time = (tithiEnd.Time - tithiStart.Time) / 4 + tithiStart.Time,
                nDst = tithiStart.nDst
            };
            tithi_quart.ApplyDstType(UtcDayStart, DstDayType);


            switch (t.Previous.nMahadvadasiID)
            {
                case MahadvadasiType.EV_UNMILANI:
                    parEnd = GCMath.Min(tithiEnd, third_day);
                    parBeg = sunRise;
                    break;
                case MahadvadasiType.EV_VYANJULI:
                    parBeg = sunRise;
                    parEnd = GCMath.Min(tithiEnd, third_day);
                    break;
                case MahadvadasiType.EV_TRISPRSA:
                    parBeg = sunRise;
                    parEnd = third_day;
                    break;
                case MahadvadasiType.EV_JAYANTI:
                case MahadvadasiType.EV_VIJAYA:
                    if (GCTithi.TITHI_DVADASI(t.astrodata.sunRise.Tithi))
                    {
                        if (naksEnd.Time < tithiEnd.Time)
                        {
                            if (naksEnd.Time < third_day.Time)
                            {
                                parBeg = naksEnd;
                                parEnd = GCMath.Min(tithiEnd, third_day);
                            }
                            else
                            {
                                parBeg = naksEnd;
                                parEnd = tithiEnd;
                            }
                        }
                        else
                        {
                            parBeg = sunRise;
                            parEnd = GCMath.Min(tithiEnd, third_day);
                        }
                    }
                    else
                    {
                        parBeg = sunRise;
                        parEnd = GCMath.Min(naksEnd, third_day);
                    }

                    break;
                case MahadvadasiType.EV_JAYA:
                case MahadvadasiType.EV_PAPA_NASINI:
                    if (GCTithi.TITHI_DVADASI(t.astrodata.sunRise.Tithi))
                    {
                        if (naksEnd.Time < tithiEnd.Time)
                        {
                            if (naksEnd.Time < third_day.Time)
                            {
                                parBeg = naksEnd;
                                parEnd = GCMath.Min(tithiEnd, third_day);
                            }
                            else
                            {
                                parBeg = naksEnd;
                                parEnd = tithiEnd;
                            }
                        }
                        else
                        {
                            parBeg = sunRise;
                            parEnd = GCMath.Min(tithiEnd, third_day);
                        }
                    }
                    else
                    {
                        if (naksEnd.Time < third_day.Time)
                        {
                            parBeg = naksEnd;
                            parEnd = third_day;
                        }
                        else
                        {
                            parBeg = naksEnd;
                            parEnd = null;
                        }
                    }

                    break;
                default:
                    // first initial
                    parEnd = GCMath.Min(tithiEnd, third_day);
                    parBeg = GCMath.Max(sunRise, tithi_quart);
                    if (GCTithi.TITHI_DVADASI(t.Previous.astrodata.sunRise.Tithi))
                    {
                        parBeg = sunRise;
                    }

                    break;
            }

            if (parBeg.Time >= parEnd.Time)
            {
                parEnd = null;
            }

            t.eparana_time1 = parBeg;
            t.eparana_time2 = parEnd;

            return 1;
        }

        public TCoreEvent FindCoreEvent(int nType)
        {
            for (int i = 0; i < coreEvents.Count; i++)
            {
                if (coreEvents[i].nType == nType)
                    return coreEvents[i];
            }

            return null;
        }

        public List<CoreEventFindRec> GetRecentCoreTimes(int nTypeStart, int nTypeFind, int count)
        {
            List<CoreEventFindRec> dayList = new List<CoreEventFindRec>();
            VAISNAVADAY cursor = this;
            int idx = 0;
            idx = coreEvents.FindIndexOf(nTypeStart, idx);
            if (idx >= 0)
            {
                while (cursor != null && dayList.Count < count)
                {
                    idx = cursor.coreEvents.FindBackIndexOf(nTypeFind, idx - 1);
                    if (idx < 0)
                    {
                        cursor = cursor.Previous;
                        idx = 1000;
                    }
                    else
                    {
                        CoreEventFindRec cer = new CoreEventFindRec();
                        cer.day = cursor;
                        cer.coreEvent = cursor.coreEvents[idx];
                        cer.dateTimeOfEvent = cursor.GetGregorianDateTime(cursor.coreEvents[idx]);
                        dayList.Insert(0, cer);
                    }
                }
            }

            return dayList;
        }


        public List<CoreEventFindRec> GetNextCoreTimes(int nTypeStart, int nTypeFind, int count)
        {
            List<CoreEventFindRec> dayList = new List<CoreEventFindRec>();
            VAISNAVADAY cursor = this;
            int idx = 0;
            idx = coreEvents.FindIndexOf(nTypeStart, idx);
            if (idx >= 0)
            {
                while (cursor != null && dayList.Count < count)
                {
                    idx = cursor.coreEvents.FindIndexOf(nTypeFind, idx++);
                    if (idx < 0)
                    {
                        cursor = cursor.Next;
                        idx = -1;
                    }
                    else
                    {
                        CoreEventFindRec cer = new CoreEventFindRec();
                        cer.day = cursor;
                        cer.coreEvent = cursor.coreEvents[idx];
                        cer.dateTimeOfEvent = cursor.GetGregorianDateTime(cursor.coreEvents[idx]);
                        dayList.Add(cer);
                    }
                }
            }

            return dayList;
        }

        public int MahadvadasiCalc(GCEarthData earth)
        {
            VAISNAVADAY t = this;

            if (t.Previous == null || t.Next == null)
                return 0;

            int nMahaType = MahadvadasiType.EV_NULL;
            //int nMhdDay = -1;

            VAISNAVADAY s = t.Previous;
            VAISNAVADAY u = t.Next;
            VAISNAVADAY mahaDay = null;

            // if yesterday is dvadasi
            // then we skip this day
            if (GCTithi.TITHI_DVADASI(s.astrodata.sunRise.Tithi))
                return 1;

            if (TithiId.TITHI_GAURA_DVADASI == t.astrodata.sunRise.Tithi && TithiId.TITHI_GAURA_DVADASI == t.astrodata.sunSet.Tithi && t.IsMhd58(out nMahaType))
            {
                t.nMahadvadasiID = nMahaType;
                mahaDay = t;
            }
            else if (GCTithi.TITHI_DVADASI(t.astrodata.sunRise.Tithi))
            {
                if (GCTithi.TITHI_DVADASI(u.astrodata.sunRise.Tithi) && GCTithi.TITHI_EKADASI(s.astrodata.sunRise.Tithi) && GCTithi.TITHI_EKADASI(s.astrodata.sunArunodaya.Tithi))
                {
                    t.nMahadvadasiID = MahadvadasiType.EV_VYANJULI;
                    mahaDay = t;
                }
                else if (t.NextNewFullIsVriddhi(earth))
                {
                    t.nMahadvadasiID = MahadvadasiType.EV_PAKSAVARDHINI;
                    mahaDay = t;
                }
                else if (GCTithi.TITHI_LESS_EKADASI(s.astrodata.sunArunodaya.Tithi))
                {
                    t.nMahadvadasiID = MahadvadasiType.EV_SUDDHA;
                    mahaDay = t;
                }
            }

            if (mahaDay != null && mahaDay.Next != null)
            {
                // fasting day
                mahaDay.nFastID = FastType.FAST_EKADASI;
                mahaDay.ekadasi_vrata_name = GCEkadasi.GetEkadasiName(t.astrodata.Masa, t.astrodata.sunRise.Paksa);
                mahaDay.ekadasi_parana = false;
                mahaDay.eparana_time1 = null;
                mahaDay.eparana_time2 = null;

                // parana day
                mahaDay.Next.nFastID = FastType.FAST_NULL;
                mahaDay.Next.ekadasi_parana = true;
                mahaDay.Next.eparana_time1 = null;
                mahaDay.Next.eparana_time2 = null;
            }

            return 1;
        }

        public bool NextNewFullIsVriddhi(GCEarthData earth)
        {
            int i = 0;
            int nTithi;
            int nPrevTithi = 100;
            VAISNAVADAY t = this;

            for (i = 0; i < 8 && t != null; i++)
            {
                nTithi = t.astrodata.sunRise.Tithi;
                if ((nTithi == nPrevTithi) && GCTithi.TITHI_FULLNEW_MOON(nTithi))
                {
                    return true;
                }
                nPrevTithi = nTithi;
                t = t.Next;
            }

            return false;
        }


        // test for MAHADVADASI 5 TO 8
        public bool IsMhd58(out int nMahaType)
        {
            VAISNAVADAY t = this;

            nMahaType = MahadvadasiType.EV_NULL;
            if (t.Next == null)
                return false;

            int sunRiseNaksatra = t.astrodata.sunRise.Naksatra;

            if (sunRiseNaksatra != t.Next.astrodata.sunRise.Naksatra)
                return false;

            if (t.astrodata.sunRise.Paksa != 1)
                return false;

            if (t.astrodata.sunRise.Tithi == t.astrodata.sunSet.Tithi)
            {
                if (sunRiseNaksatra == GCNaksatra.N_PUNARVASU) // punarvasu
                {
                    nMahaType = MahadvadasiType.EV_JAYA;
                    return true;
                }
                else if (sunRiseNaksatra == GCNaksatra.N_ROHINI) // rohini
                {
                    nMahaType = MahadvadasiType.EV_JAYANTI;
                    return true;
                }
                else if (sunRiseNaksatra == GCNaksatra.N_PUSYAMI) // pusyami
                {
                    nMahaType = MahadvadasiType.EV_PAPA_NASINI;
                    return true;
                }
                else if (sunRiseNaksatra == GCNaksatra.N_SRAVANA) // sravana
                {
                    nMahaType = MahadvadasiType.EV_VIJAYA;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (sunRiseNaksatra == GCNaksatra.N_SRAVANA) // sravana
                {
                    nMahaType = MahadvadasiType.EV_VIJAYA;
                    return true;
                }
            }

            return false;
        }

        //public bool GetTithiTimeRange(GCEarthData earth, out GregorianDateTime from, out GregorianDateTime to)
        //{
        //    GregorianDateTime start = new GregorianDateTime();

        //    start.Set(date);
        //    start.shour = astrodata.sunRise.TotalDays;

        //    GCTithi.GetNextTithiStart(earth, start, out to);
        //    GCTithi.GetPrevTithiStart(earth, start, out from);

        //    return true;

        //}

        //public bool GetNaksatraTimeRange(GCEarthData earth, out GregorianDateTime from, out GregorianDateTime to)
        //{
        //    GregorianDateTime start = new GregorianDateTime();

        //    start.Set(date);
        //    start.shour = astrodata.sunRise.TotalDays;

        //    GCNaksatra.GetNextNaksatra(earth, start, out to);
        //    GCNaksatra.GetPrevNaksatra(earth, start, out from);

        //    return true;
        //}

        public string GetTextEP()
        {
            string str = string.Empty;
            GregorianDateTime e1, e2;

            e1 = GetGregorianDateTime(eparana_time1);

            if (eparana_time2 != null)
            {
                e2 = GetGregorianDateTime(eparana_time2);

                if (GCDisplaySettings.Current.getValue(50) == 1)
                    str = string.Format("{0} {1} ({2}) - {3} ({4}) {5}", GCStrings.getString(60),
                        e1.ShortTimeString(), GCEkadasi.GetParanaReasonText(eparana_time1.nType),
                        e2.ShortTimeString(), GCEkadasi.GetParanaReasonText(eparana_time2.nType),
                        GCStrings.GetDSTSignature(BiasMinutes));
                else
                    str = string.Format("{0} {1} - {2} ({3})", GCStrings.getString(60),
                        e1.ShortTimeString(), e2.ShortTimeString(), GCStrings.GetDSTSignature(BiasMinutes));
            }
            else if (eparana_time1 != null)
            {
                if (GCDisplaySettings.Current.getValue(50) == 1)
                    str = string.Format("{0} {1} ({2}) {3}", GCStrings.getString(61),
                        e1.ShortTimeString(), GCEkadasi.GetParanaReasonText(eparana_time1.nType), GCStrings.GetDSTSignature(BiasMinutes));
                else
                    str = string.Format("{0} {1} ({2})", GCStrings.getString(61),
                        e1.ShortTimeString(), GCStrings.GetDSTSignature(BiasMinutes));
            }
            else
            {
                str = GCStrings.getString(62);
            }
            return str;
        }

        public string GetDateText()
        {
            return string.Format("{0:00} {1} {2} {3}", date.day, GregorianDateTime.GetMonthAbreviation(date.month), 
                date.year, GCCalendar.GetWeekdayAbbr(date.dayOfWeek));
        }

        bool hasEventsOfDisplayIndex(int dispIndex)
        {
            foreach (VAISNAVAEVENT md in dayEvents)
            {
                if (md.dispItem == dispIndex)
                    return true;
            }

            return false;
        }


        public VAISNAVAEVENT findEventsText(string text)
        {

            foreach (VAISNAVAEVENT md in dayEvents)
            {
                if (md.text != null && md.text.IndexOf(text, 0, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    return md;
            }

            return null;
        }


        public VAISNAVAEVENT AddEvent(int priority, int dispItem, string text)
        {
            VAISNAVAEVENT dc = new VAISNAVAEVENT();

            dc.prio = priority;
            dc.dispItem = dispItem;
            dc.text = text;

            dayEvents.Add(dc);

            return dc;
        }

        public string Format(string format, params string[] args)
        {
            StringBuilder sb = new StringBuilder(format);

            if (format.IndexOf("{day}") >= 0)
                format = format.Replace("{day}", date.day.ToString());
            if (format.IndexOf("{month}") >= 0)
                format = format.Replace("{month}", date.month.ToString());
            if (format.IndexOf("{monthAbr}") >= 0)
                format = format.Replace("{monthAbr}", GregorianDateTime.GetMonthName(date.month));
            if (format.IndexOf("{monthName}") >= 0)
                format = format.Replace("{monthName}", GregorianDateTime.GetMonthName(date.month));
            if (format.IndexOf("{year}") >= 0)
                format = format.Replace("{year}", date.year.ToString());
            if (format.IndexOf("{hour}") >= 0)
                format = format.Replace("{hour}", date.GetHour().ToString("D2"));
            if (format.IndexOf("{min}") >= 0)
                format = format.Replace("{min}", date.GetMinute().ToString("D2"));
            if (format.IndexOf("{minRound}") >= 0)
                format = format.Replace("{minRound}", date.GetMinuteRound().ToString("D2"));
            if (format.IndexOf("{sec}") >= 0)
                format = format.Replace("{sec}", date.GetSecond().ToString("D2"));

            if (format.IndexOf("{masaName}") >= 0)
                format = format.Replace("{masaName}", GCMasa.GetName(astrodata.Masa));
            if (format.IndexOf("{gaurabdaYear}") >= 0)
                format = format.Replace("{gaurabdaYear}", astrodata.GaurabdaYear.ToString());
            if (format.IndexOf("{tithiName}") >= 0)
                format = format.Replace("{tithiName}", GCTithi.GetName(astrodata.sunRise.Tithi));
            if (format.IndexOf("{prevTithiName}") >= 0)
                format = format.Replace("{prevTithiName}", GCTithi.GetName((astrodata.sunRise.Tithi + 29) % 30));
            if (format.IndexOf("{nextTithiName}") >= 0)
                format = format.Replace("{nextTithiName}", GCTithi.GetName((astrodata.sunRise.Tithi + 1) % 30));
            if (format.IndexOf("{paksaName}") >= 0)
                format = format.Replace("{paksaName}", GCPaksa.GetName(astrodata.sunRise.Paksa));
            if (format.IndexOf("{yogaName}") >= 0)
                format = format.Replace("{yogaName}", GCYoga.GetName(astrodata.sunRise.Yoga));
            if (format.IndexOf("{naksatraName}") >= 0)
                format = format.Replace("{naksatraName}", GCNaksatra.GetName(astrodata.sunRise.Naksatra));
            if (format.IndexOf("{naksatraElapse}") >= 0)
                format = format.Replace("{naksatraElapse}", astrodata.sunRise.NaksatraElapse.ToString("P2"));
            if (format.IndexOf("{naksatraPada}") >= 0)
                format = format.Replace("{naksatraPada}", GCNaksatra.GetPadaText(astrodata.sunRise.NaksatraPada));

            if (format.IndexOf("{sankranti.day}") >= 0)
                format = format.Replace("{sankranti.day}", sankranti_day.day.ToString());
            if (format.IndexOf("{sankranti.month}") >= 0)
                format = format.Replace("{sankranti.month}", sankranti_day.month.ToString());
            if (format.IndexOf("{sankranti.monthAbr}") >= 0)
                format = format.Replace("{sankranti.monthAbr}", GregorianDateTime.GetMonthName(sankranti_day.month));
            if (format.IndexOf("{sankranti.monthName}") >= 0)
                format = format.Replace("{sankranti.monthName}", GregorianDateTime.GetMonthName(sankranti_day.month));
            if (format.IndexOf("{sankranti.hour}") >= 0)
                format = format.Replace("{sankranti.hour}", sankranti_day.GetHour().ToString("D2"));
            if (format.IndexOf("{sankranti.min}") >= 0)
                format = format.Replace("{sankranti.min}", sankranti_day.GetMinute().ToString("D2"));
            if (format.IndexOf("{sankranti.minRound}") >= 0)
                format = format.Replace("{sankranti.minRound}", sankranti_day.GetMinuteRound().ToString("D2"));
            if (format.IndexOf("{sankranti.sec}") >= 0)
                format = format.Replace("{sankranti.sec}", sankranti_day.GetSecond().ToString("D2"));
            if (format.IndexOf("{sankranti.rasiNameEn}") >= 0)
                format = format.Replace("{sankranti.rasiNameEn}", GCRasi.GetNameEn(sankranti_zodiac));
            if (format.IndexOf("{sankranti.rasiName}") >= 0)
                format = format.Replace("{sankranti.rasiName}", GCRasi.GetName(sankranti_zodiac));

            if (format.IndexOf("{dstSig}") >= 0)
                format = format.Replace("{dstSig}", GCStrings.GetDSTSignature(BiasMinutes));

            if (format.IndexOf("{moonRiseTime}") >= 0)
                format = format.Replace("{moonRiseTime}", moonrise.ToShortTimeString());
            if (format.IndexOf("{moonSetTime}") >= 0)
                format = format.Replace("{moonSetTime}", moonset.ToShortTimeString());
            if (format.IndexOf("{moonRasiName}") >= 0)
                format = format.Replace("{moonRasiName}", GCRasi.GetName(astrodata.sunRise.RasiOfMoon));
            if (format.IndexOf("{moonRasiNameEn}") >= 0)
                format = format.Replace("{moonRasiNameEn}", GCRasi.GetNameEn(astrodata.sunRise.RasiOfMoon));

            if (args == null || args.Length == 0)
                return format.ToString();
            else
                return string.Format(format.ToString(), args);
        }

        public String GetFullTithiName()
        {
            string str;
            str = GCTithi.GetName(astrodata.sunRise.Tithi);

            if (HasExtraFastingNote())
            {
                str = string.Format("{0} {1}", str, GetExtraFastingNote());
            }

            return str;
        }

        public bool HasExtraFastingNote()
        {
            int t = astrodata.sunRise.Tithi;
            if ((t == 10) || (t == 25) || (t == 11) || (t == 26))
            {
                if (ekadasi_parana == false)
                {
                    return true;
                }
            }

            return false;
        }

        public string GetExtraFastingNote()
        {
            if (HasExtraFastingNote())
            {
                if (nMahadvadasiID == MahadvadasiType.EV_NULL)
                {
                    return GCStrings.getString(58);
                }
                else
                {
                    return GCStrings.getString(59);
                }
            }
            return "";
        }

    }

    public class VAISNAVAEVENTComparer : Comparer<VAISNAVAEVENT>
    {
        public override int Compare(VAISNAVAEVENT x, VAISNAVAEVENT y)
        {
            return x.prio - y.prio;
        }
    }
}
