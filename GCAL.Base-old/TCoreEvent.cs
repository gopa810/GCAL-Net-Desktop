using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public class TCoreEvent : GSCore
    {
        public byte nType;
        public byte nData;
        public long Time;
        public long GetDstTime(int biasSeconds)
        {
            return Time + nDst * biasSeconds;
        }

        public byte nDst;

        public TCoreEvent()
        {

        }

        public TCoreEvent(TCoreEvent tce)
        {
            Set(tce);
        }

        void Set(TCoreEvent de)
        {
            nType = de.nType;
            nData = de.nData;
            Time = de.Time;
            nDst = de.nDst;
        }

        public int DaySeconds(long utcDayStart)
        {
            return Convert.ToInt32((Time - utcDayStart + 86400) % 86400);
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
                    return new GSNumber(Time);
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
                case CoreEventType.CCTYPE_CONJ:
                    return "Conjunction";
                case CoreEventType.CCTYPE_DATE:
                    return "Date";
                case CoreEventType.CCTYPE_M_RASI:
                    return "Moon rasi";
                case CoreEventType.CCTYPE_NAKS:
                    return "Naksatra";
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

        public void ApplyDstType(long utcDayStart, DstTypeChange inDst)
        {
            switch (inDst)
            {
                case DstTypeChange.DstOff:
                    this.nDst = 0;
                    break;
                case DstTypeChange.DstStart:
                    if (this.DaySeconds(utcDayStart) >= 7200)
                    {
                        this.nDst = 1;
                    }
                    else
                    {
                        this.nDst = 0;
                    }
                    break;
                case DstTypeChange.DstOn:
                    this.nDst = 1;
                    break;
                case DstTypeChange.DstEnd:
                    if (this.DaySeconds(utcDayStart) <= 7200)
                    {
                        this.nDst = 1;
                    }
                    else
                    {
                        this.nDst = 0;
                    }
                    break;
                default:
                    this.nDst = 0;
                    break;
            }
        }

    }

}
