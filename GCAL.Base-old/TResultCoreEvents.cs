using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public class TResultCoreEvents: GSCore
    {
        public bool Full = false;
        public int Year = 2020;
        public GCLocation EarthLocation = new GCLocation();

        public void Clear()
        {
            p_events.Clear();
        }

        public List<TCoreEvent> p_events = new List<TCoreEvent>();

        public override GSCore GetPropertyValue(string s)
        {
            if (s.Equals("items"))
            {
                GSList list = new GSList();
                foreach (TCoreEvent de in p_events)
                {
                    list.Add(de);
                }
                return list;
            }
            else if (s.Equals("year"))
                return new GSNumber(Year);
            else
            {
                return base.GetPropertyValue(s);
            }
        }

        public bool AddEvent(GregorianDateTime gTime, int inType, int inData)
        {
            TCoreEvent de = new TCoreEvent();

            Int64 inTime = Convert.ToInt64(gTime.GetJulianComplete() * 86400);

            de.Time = inTime;
            de.nDst = 0;
            de.nData = (byte)inData;
            de.nType = (byte)inType;

            p_events.Add(de);

            return true;
        }


        public void Sort()
        {
            TCoreEventComparer dec = new TCoreEventComparer();
            p_events.Sort(dec);
        }

        public TCoreEvent this[int i]
        {
            get
            {
                return p_events[i];
            }
        }
        public TResultCoreEvents()
        {
            p_events.Clear();
        }

        public bool LoadFile(string filePath)
        {
            try
            {
                using (Stream s = File.OpenRead(filePath))
                {
                    using (BinaryReader br = new BinaryReader(s))
                    {
                        p_events.Clear();
                        byte b = br.ReadByte();
                        if (b == 1) // read version 1
                        {
                            b = br.ReadByte();
                            while(b != 0)
                            {
                                switch(b)
                                {
                                    case 10: // tag for year
                                        Year = br.ReadInt32();
                                        break;
                                    case 60: // tag for core event
                                        TCoreEvent ce = new TCoreEvent();
                                        ce.nType = br.ReadByte();
                                        ce.nData = br.ReadByte();
                                        ce.nDst = br.ReadByte();
                                        ce.Time = br.ReadInt64();
                                        p_events.Add(ce);
                                        break;
                                    default:
                                        break;
                                }
                                b = br.ReadByte();
                            }
                        }
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                GCLog.Write("Error during loading CoreEvents file.");
                GCLog.Write(ex.Message);
                return false;
            }
        }

        public bool SaveFile(string filePath)
        {
            try
            {
                using (Stream s = File.OpenWrite(filePath))
                {
                    using (BinaryWriter bw = new BinaryWriter(s))
                    {
                        // signature of version
                        bw.Write((byte)1);

                        // tag 10 for year
                        bw.Write((byte)10);
                        bw.Write((int)Year);

                        foreach (TCoreEvent ce in p_events)
                        {
                            // tag 60 for record of event
                            bw.Write((byte)60);
                            // write one day event
                            bw.Write(ce.nType);
                            bw.Write(ce.nData);
                            bw.Write(ce.nDst);
                            bw.Write(ce.Time);
                        }

                        // end tag
                        bw.Write((byte)0);
                    }
                }

                return true;
            }
            catch(Exception e)
            {
                GCLog.Write("Error during saving CoreEvents file.");
                GCLog.Write(e.Message);
                return false;
            }
        }

        public void CalculateEvents(GCLocation loc, int nYear)
        {
            //GCSunData sun = new GCSunData();
            //DstTypeChange ndst = DstTypeChange.DstOff;
            int nData;

            TResultCoreEvents inEvents = this;
            this.Clear();
            this.EarthLocation = loc;
            this.Year = nYear;
            GregorianDateTime vcStart = new GregorianDateTime(Year - 1, 12, 29);
            GregorianDateTime vcEnd = new GregorianDateTime(Year + 1, 1, 2);

            GregorianDateTime vc = new GregorianDateTime();
            GregorianDateTime vcAdd = new GregorianDateTime(), vcNext = new GregorianDateTime();
            GCEarthData earth = loc.GetEarthData();

            vc.Set(vcStart);
            vc.TimezoneHours = loc.OffsetUtcHours;
            vcAdd.Set(vc);
            vcAdd.InitWeekDay();

            /*while (vcAdd.IsBeforeThis(vcEnd))
            {
                ndst = loc.TimeZone.DetermineDaylightChange(vcAdd);
                vcAdd.NextDay();
            }*/


            if (Full || GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_TITHI) != 0)
            {
                vcAdd.Set(vc);
                vcAdd.shour = 0.0;
                while (vcAdd.IsBeforeThis(vcEnd))
                {
                    nData = GCTithi.GetNextTithiStart(earth, vcAdd, out vcNext);
                    if (vcNext.GetDayInteger() < vcEnd.GetDayInteger())
                    {
                        //vcNext.InitWeekDay();
                        //ndst = loc.TimeZone.DetermineDaylightChange(vcNext);
                        inEvents.AddEvent(vcNext, CoreEventType.CCTYPE_TITHI, nData);
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

            if (Full || GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_NAKSATRA) != 0)
            {
                vcAdd.Set(vc);
                vcAdd.shour = 0.0;
                while (vcAdd.IsBeforeThis(vcEnd))
                {
                    nData = GCNaksatra.GetNextNaksatra(earth, vcAdd, out vcNext);
                    if (vcNext.GetDayInteger() < vcEnd.GetDayInteger())
                    {
                        //vcNext.InitWeekDay();
                        //ndst = loc.TimeZone.DetermineDaylightChange(vcNext);
                        inEvents.AddEvent(vcNext, CoreEventType.CCTYPE_NAKS, nData);
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

            if (Full || GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_YOGA) != 0)
            {
                vcAdd.Set(vc);
                vcAdd.shour = 0.0;
                while (vcAdd.IsBeforeThis(vcEnd))
                {
                    nData = GCYoga.GetNextYogaStart(earth, vcAdd, out vcNext);
                    if (vcNext.GetDayInteger() < vcEnd.GetDayInteger())
                    {
                        //vcNext.InitWeekDay();
                        //ndst = loc.TimeZone.DetermineDaylightChange(vcNext);
                        inEvents.AddEvent(vcNext, CoreEventType.CCTYPE_YOGA, nData);
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

            if (Full || GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_SANKRANTI) != 0)
            {
                vcNext = new GregorianDateTime();
                vcAdd.Set(vc);
                vcAdd.shour = 0.0;
                while (vcAdd.IsBeforeThis(vcEnd))
                {
                    vcNext.Set(GCSankranti.GetNextSankranti(vcAdd, earth, out nData));
                    if (vcNext.GetDayInteger() < vcEnd.GetDayInteger())
                    {
                        //vcNext.InitWeekDay();
                        //ndst = loc.TimeZone.DetermineDaylightChange(vcNext);
                        inEvents.AddEvent(vcNext, CoreEventType.CCTYPE_SANK, nData);
                    }
                    else
                    {
                        break;
                    }
                    vcAdd.Set(vcNext);
                    vcAdd.NextDay();
                }
            }

            if (Full || GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_MOONRASI) != 0)
            {
                vcAdd.Set(vc);
                vcAdd.shour = 0.0;
                while (vcAdd.IsBeforeThis(vcEnd))
                {
                    nData = GCMoonData.GetNextMoonRasi(earth, vcAdd, out vcNext);
                    if (vcNext.GetDayInteger() < vcEnd.GetDayInteger())
                    {
                        //vcNext.InitWeekDay();
                        //ndst = loc.TimeZone.DetermineDaylightChange(vcNext);
                        inEvents.AddEvent(vcNext, CoreEventType.CCTYPE_M_RASI, nData);
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
            if (Full || GCDisplaySettings.Current.getValue(GCDS.COREEVENTS_CONJUNCTION) != 0)
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
                        //ndst = loc.TimeZone.DetermineDaylightChange(vcNext);
                        inEvents.AddEvent(vcNext, CoreEventType.CCTYPE_CONJ, GCRasi.GetRasi(dlong, GCAyanamsha.GetAyanamsa(vcNext.GetJulianComplete())));
                    }
                    else
                    {
                        break;
                    }
                    vcAdd.Set(vcNext);
                    vcAdd.NextDay();
                }
            }

            inEvents.Sort();
        }

        public void GetCoreEvents(List<TCoreEvent> coreEvents, long utcDayStart, long utcDayEnd)
        {
            if (p_events.Count == 0)
                return;

            TCoreEvent first = p_events[0];
            TCoreEvent last = p_events[p_events.Count - 1];

            if (first.Time > utcDayEnd || last.Time < utcDayStart || first.Time == last.Time)
                return;

            int index =  Convert.ToInt32((utcDayStart - first.Time) * 1.0 * p_events.Count / (last.Time - first.Time));
            int max = p_events.Count - 1;
            while (index > 0 && index <= max && p_events[index].Time > utcDayStart)
                index--;

            while (index > 0 && index < max && p_events[index].Time < utcDayStart)
                index++;

            while(index > 0 && index < max && p_events[index].Time < utcDayEnd)
            {
                coreEvents.Add(p_events[index]);
                index++;
            }
        }
    }

    public sealed class CoreEventType
    {
        public const int CCTYPE_DATE = 1;
        public const int CCTYPE_S_ARUN = 10;
        public const int CCTYPE_S_RISE = 11;
        public const int CCTYPE_S_NOON = 12;
        public const int CCTYPE_S_SET = 13;
        public const int CCTYPE_S_MIDNIGHT = 14;

        public const int CCTYPE_TITHI = 20;
        public const int CCTYPE_NAKS = 21;
        public const int CCTYPE_SANK = 22;
        public const int CCTYPE_CONJ = 23;
        public const int CCTYPE_YOGA = 24;
        public const int CCTYPE_KALA_START = 30;
        public const int CCTYPE_KALA_END = 31;
        public const int CCTYPE_M_RISE = 41;
        public const int CCTYPE_M_SET = 42;
        public const int CCTYPE_M_RASI = 45;
        public const int CCTYPE_ASCENDENT = 50;

        public const int CCTYPE_TITHI_BASE = 60;
        public const int CCTYPE_DAY_MUHURTA = 61;
        public const int CCTYPE_DAY_OF_WEEK = 62;
        public const int CCTYPE_THIRD_OF_DAY = 63;
        public const int CCTYPE_TITHI_QUARTER = 64;

        public const int CCTYPE_NAKS_PADA1 = 65;
        public const int CCTYPE_NAKS_PADA2 = 66;
        public const int CCTYPE_NAKS_PADA3 = 67;
        public const int CCTYPE_NAKS_PADA4 = 68;
        public const int CCTYPE_NAKS_END = 69;
        public const int CCTYPE_TITHI_END = 70;

    };

    public class TCoreEventCollection: List<TCoreEvent>
    {
        public void InsertByTime(TCoreEvent coreEvent)
        {
            for(int i = 0; i < this.Count; i++)
            {
                if (this[i].Time > coreEvent.Time)
                {
                    this.Insert(i, coreEvent);
                    return;
                }
            }

            this.Add(coreEvent);
        }

        public int FindIndexOf(int nType, int idx)
        {
            if (idx < 0)
                idx = 0;
            for(int i = idx; i < Count; i++)
            {
                if (this[i].nType == nType)
                    return i;
            }

            return -1;
        }

        public int FindBackIndexOf(int nType, int idx)
        {
            if (idx >= Count)
                idx = Count - 1;
            if (idx < 0)
                return -1;

            for(int i = idx; i >= 0; i--)
            {
                if (this[i].nType == nType)
                    return i;
            }

            return -1;
        }
    }

    public class TCoreEventComparer : Comparer<TCoreEvent>
    {
        public override int Compare(TCoreEvent x, TCoreEvent y)
        {
            Int64 d = (Int64)x.Time - (Int64)y.Time;
            if (d < 0)
                return -1;
            if (d > 0)
                return +1;
            return 0;
        }
    }

}
