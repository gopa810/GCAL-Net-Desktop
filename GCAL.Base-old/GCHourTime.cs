using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public struct GCHourTime
    {
        public int hour;
        public int min;
        public int sec;
        public int mili;

        public double longitude;
        public double longitudeMoon;
        public double Ayanamsa;

        public GCHourTime(GCHourTime rise, int p)
        {
            // TODO: Complete member initialization
            hour = rise.hour;
            min = rise.min;
            sec = rise.sec;
            mili = rise.mili;
            longitude = 0.0;
            longitudeMoon = 0.0;
            Ayanamsa = 0.0;
            AddMinutes(p);
        }

        public int Tithi
        {
            get
            {
                double d = GCMath.putIn360(longitudeMoon - longitude - 180.0) / 12.0;
                return GCMath.IntFloor(d);
            }
        }

        public double TithiElapse
        {
            get
            {
                double d = GCMath.putIn360(longitudeMoon - longitude - 180.0) / 12.0;
                return (d - Math.Floor(d)) * 100.0;
            }
        }

        public int Paksa
        {
            get
            {
                return Tithi >= 15 ? 1 : 0;
            }
        }

        public int Naksatra
        {
            get
            {
                double d = GCMath.putIn360(longitudeMoon - Ayanamsa);
                d = (d * 3.0) / 40.0;
                return GCMath.IntFloor(d);
            }
        }

        public double NaksatraElapse
        {
            get
            {
                double d = GCMath.putIn360(longitudeMoon - Ayanamsa);
                d = (d * 3.0) / 40.0;
                return (d - Math.Floor(d)) * 100.0;
            }
        }

        public int NaksatraPada
        {
            get
            {
                return GCMath.IntFloor(NaksatraElapse / 25.0);
            }
        }


        public int Yoga
        {
            get
            {
                double d = GCMath.putIn360(longitudeMoon + longitude - 2 * Ayanamsa);
                d = (d * 3.0) / 40.0;
                return GCMath.IntFloor(d);
            }
        }

        public double YogaElapse
        {
            get
            {
                double d = GCMath.putIn360(longitudeMoon + longitude - 2 * Ayanamsa);
                d = (d * 3.0) / 40.0;
                return (d - Math.Floor(d)) * 100.0;
            }
        }
        
        public int RasiOfSun
        {
            get
            {
                return GCRasi.GetRasi(longitude, Ayanamsa);
            }
        }

        public int RasiOfMoon
        {
            get
            {
                return GCRasi.GetRasi(longitudeMoon, Ayanamsa);
            }
        }


        public double TotalDays
        {
            get
            {
                return hour / 24.0 + min / 1440.0 + sec / 86400.0 + mili / 86400000.0;
            }
            set
            {
                SetDayTime(value);
            }
        }

        public double TotalHours
        {
            get
            {
                return hour + min / 60.0 + sec / 3600.0 + mili / 3600000.0;
            }
        }

        public double TotalMinutes
        {
            get
            {
                return hour * 60.0 + min + sec / 60.0 + mili / 60000.0;
            }
        }

        public double TotalSeconds
        {
            get
            {
                return hour * 3600.0 + min * 60.0 + sec + mili / 1000.0;
            }
        }

        public string ShortSandhyaString()
        {
            GCHourTime start, end;

            start = new GCHourTime(this, -96);
            end = new GCHourTime(this, -48);

            return string.Format("{0}-{1}", start.ToShortTimeString(), end.ToShortTimeString());
        }

        public string ShortMuhurtaString(int nMuhurta)
        {
            GCHourTime start, end;

            start = new GCHourTime(this, nMuhurta*48);
            end = new GCHourTime(this, nMuhurta*48 + 48);

            return string.Format("{0}-{1}", start.ToShortTimeString(), end.ToShortTimeString());
        }

        public bool IsGreaterThan(GCHourTime dt)
        {
            if (hour > dt.hour)
                return true;
            else if (hour < dt.hour)
                return false;

            if (min > dt.min)
                return true;
            else if (min < dt.min)
                return false;

            if (sec > dt.sec)
                return true;
            else if (sec < dt.sec)
                return false;

            if (mili > dt.mili)
                return true;

            return false;
        }


        public bool IsLessThan(GCHourTime dt)
        {
            if (hour < dt.hour)
                return true;
            else if (hour > dt.hour)
                return false;

            if (min < dt.min)
                return true;
            else if (min > dt.min)
                return false;

            if (sec < dt.sec)
                return true;
            else if (sec > dt.sec)
                return false;

            if (mili < dt.mili)
                return true;

            return false;
        }

        public bool IsGreaterOrEqualThan(GCHourTime dt)
        {
            if (hour >= dt.hour)
                return true;
            else if (hour < dt.hour)
                return false;

            if (min >= dt.min)
                return true;
            else if (min < dt.min)
                return false;

            if (sec >= dt.sec)
                return true;
            else if (sec < dt.sec)
                return false;

            if (mili >= dt.mili)
                return true;

            return false;
        }

        public bool IsLessOrEqualThan(GCHourTime dt)
        {
            if (hour <= dt.hour)
                return true;
            else if (hour > dt.hour)
                return false;

            if (min <= dt.min)
                return true;
            else if (min > dt.min)
                return false;

            if (sec <= dt.sec)
                return true;
            else if (sec > dt.sec)
                return false;

            if (mili <= dt.mili)
                return true;

            return false;
        }


        public void AddMinutes(double mn)
        {
            min += Convert.ToInt32(Math.Floor(mn));
            while (min < 0) { min += 60; hour--; }
            while (min > 59) { min -= 60; hour++; }
        }

        public double GetDayTime()
        {
            return TotalDays;
        }

        public double GetDayTime(double DstOffsetHours)
        {
            return ((Convert.ToDouble(hour + DstOffsetHours) * 60.0 + Convert.ToDouble(min)) * 60.0 + Convert.ToDouble(sec)) / 86400.0;
        }


        public void SetValue(int i)
        {
            hour = min = sec = mili = i;
        }

        public void Set(GCHourTime d)
        {
            hour = d.hour;
            min = d.min;
            sec = d.sec;
            mili = d.mili;
        }

        public void SetDayTime(double d)
        {
            double time_hr = 0.0;

            // hour
            time_hr = d * 24;
            hour = Convert.ToInt32(Math.Floor(time_hr));

            // minute
            time_hr -= hour;
            time_hr *= 60;
            min = Convert.ToInt32(Math.Floor(time_hr));

            // second
            time_hr -= min;
            time_hr *= 60;
            sec = Convert.ToInt32(Math.Floor(time_hr));

            // miliseconds
            time_hr -= sec;
            time_hr *= 1000;
            mili = Convert.ToInt32(Math.Floor(time_hr));
        }

        ////////////////////////////////////////////////////////////////
        //
        //  Conversion time from DEGREE fromat to H:M:S:MS format
        //
        //  time - output
        //  time_deg - input time in range 0 deg to 360 deg
        //             where 0 deg = 0:00 AM and 360 deg = 12:00 PM
        //
        public void SetDegTime(double time_deg)
        {
            double time_hr = 0.0;

            time_deg = GCMath.putIn360(time_deg);

            // hour
            time_hr = time_deg / 360 * 24;
            hour = Convert.ToInt32(Math.Floor(time_hr));

            // minute
            time_hr -= hour;
            time_hr *= 60;
            min = Convert.ToInt32(Math.Floor(time_hr));

            // second
            time_hr -= min;
            time_hr *= 60;
            sec = Convert.ToInt32(Math.Floor(time_hr));

            // miliseconds
            time_hr -= sec;
            time_hr *= 1000;
            mili = Convert.ToInt32(Math.Floor(time_hr));
        }

        public string ToLongTimeString()
        {
            return string.Format("{0:00}:{1:00}:{2:00}", hour, min, sec);
        }

        public string ToShortTimeString()
        {
            return string.Format("{0:00}:{1:00}", hour, min);
        }

    }

    public class GCHourTimeObject : GSCore
    {
        public GCHourTime Value;

        public GCHourTimeObject()
        {
            Value = new GCHourTime();
        }

        public GCHourTimeObject(GCHourTime v)
        {
            Value = v;
        }

        public override GSCore GetPropertyValue(string s)
        {
            switch (s)
            {
                case "shortTime":
                    return new GSString(Value.ToShortTimeString());
                case "longTime":
                    return new GSString(Value.ToLongTimeString());
                case "standardTimeString":
                    return new GSString(String.Format("{0:00}{1:00}{2:00}", Value.hour, Value.min, Value.sec));
                default:
                    break;
            }
            return base.GetPropertyValue(s);
        }

        public override GSCore ExecuteMessage(string token, GSCoreCollection args)
        {
            if (token.Equals("addMinutes"))
            {
                Value.AddMinutes((int)args.getSafe(0).getIntegerValue());
                return this;
            }
            else
            {
                return base.ExecuteMessage(token, args);
            }
        }
    }
}
