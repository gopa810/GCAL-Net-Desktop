using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GCAL.Base
{
    public class GCConfigRatedEvents
    {
        // daily parts
        // dayhours indexes: 0 - morning (sunrise to noon), 1 - afternoon (noon to sunset)
        // 2 - sunset to midnight, 3 - mignight to sunrise
        public GCConfigRatedEntry[] rateDayHours = new GCConfigRatedEntry[4];
        public GCConfigRatedEntry[] rateDay = new GCConfigRatedEntry[2];
        public GCConfigRatedEntry[] rateMuhurta = new GCConfigRatedEntry[30];
        // rating for rateKalas[0] must be 0.0, index is from enum type KalaType
        public GCConfigRatedEntry[] rateKalas = new GCConfigRatedEntry[6];

        // moon rise/set
        public GCConfigRatedEntry[] rateMoonTime = new GCConfigRatedEntry[2];

        // weekly parts
        public GCConfigRatedEntry[] weekday = new GCConfigRatedEntry[7];

        // monthly parts
        public GCConfigRatedEntry[] rateTithi = new GCConfigRatedEntry[30];
        public GCConfigRatedEntry[] rateNaksatra = new GCConfigRatedEntry[27];
        public GCConfigRatedEntry[,] rateNaksatraPada = new GCConfigRatedEntry[27, 4];
        public GCConfigRatedEntry[] rateYoga = new GCConfigRatedEntry[27];
        
        // grahas
        public GCConfigRatedEntry[,] rateGrahaRasi = new GCConfigRatedEntry[10, 12];
        public GCConfigRatedEntry[,] rateGrahaHouse = new GCConfigRatedEntry[10, 12];
        public GCConfigRatedEntry[,] rateRasiGraha = new GCConfigRatedEntry[12, 10];
        public GCConfigRatedEntry[,] rateHouseGraha = new GCConfigRatedEntry[12, 10];

        /// <summary>
        /// Flag indicating that only periods with rating higher than given value
        /// will be presented in results
        /// </summary>
        public bool useAcceptLimit = false;
        /// <summary>
        /// minimal rating for displaying results
        /// </summary>
        public double acceptLimit = 0.0;

        /// <summary>
        /// Flag indicating that only periods longer than given value will
        /// be presented in results
        /// </summary>
        public bool useMinPeriodLength = true;
        /// <summary>
        /// Length of minimal period in minutes
        /// </summary>
        public double minPeriodLength = 24.0;
        /// <summary>
        /// Title of this configuration for rated events
        /// </summary>
        public string Title = "";

        /// <summary>
        /// short description up to 255 characters
        /// </summary>
        public string Description = "";

        public delegate string getParamStringDlg(int i);

        /// <summary>
        /// Private class for combined references to arrays
        /// </summary>
        public class ParameterDescriptor
        {
            public string Key = null;
            public int Min = 0;
            public int Min2 = 0;
            public int Max = 0;
            public int Max2 = 0;
            public bool Additional = false;
            public GCConfigRatedEntry[] Array = null;
            public GCConfigRatedEntry[,] Array2 = null;
            public getParamStringDlg param1Func = null;
            public getParamStringDlg param2Func = null;
        }

        private List<ParameterDescriptor> p_paramdesc = null;

        public GCConfigRatedEvents()
        {
            int i;
            for (i = 0; i < 30; i++)
            {
                rateTithi[i] = new GCConfigRatedEntry();
                rateTithi[i].Title = string.Format("{0} Tithi", GCTithi.GetName(i));
                rateTithi[i].Key = "Tithi";
                rateMuhurta[i] = new GCConfigRatedEntry();
                rateMuhurta[i].Title = GCStrings.GetMuhurtaName(i);
                rateMuhurta[i].Key = "Muhurta";
            }

            for (i = 0; i < 4; i++)
            {
                rateDayHours[i] = new GCConfigRatedEntry();
                rateDayHours[i].Title = GCStrings.GetSandhyaName(i);
                rateDayHours[i].Key = "DayHours";
            }

            for (i = 0; i < 2; i++)
            {
                rateDay[i] = new GCConfigRatedEntry();
                rateDay[i].Title = GCStrings.GetSandhyaName(i*2);
                rateDay[i].Key = "DayPart";
                rateMoonTime[i] = new GCConfigRatedEntry();
                rateMoonTime[i].Title = GCStrings.GetMoonTimesName(i);
                rateMoonTime[i].Key = "MoonTimes";
            }

            rateKalas[0] = new GCConfigRatedEntry();
            for (i = 1; i < 6; i++)
            {
                rateKalas[i] = new GCConfigRatedEntry();
                rateKalas[i].Title = GCStrings.GetKalaName(i);
                rateKalas[i].Key = string.Format("kala.{0}", i);
            }

            for (i = 0; i < 27; i++)
            {
                rateNaksatra[i] = new GCConfigRatedEntry();
                rateNaksatra[i].Title = string.Format("{0} Naksatra", GCNaksatra.GetName(i));
                rateNaksatra[i].Key = "Naksatra";
                rateYoga[i] = new GCConfigRatedEntry();
                rateYoga[i].Title = string.Format("{0} Yoga", GCYoga.GetName(i));
                rateYoga[i].Key = "Yoga";
                for (int j = 0; j < 4; j++)
                {
                    rateNaksatraPada[i, j] = new GCConfigRatedEntry();
                    rateNaksatraPada[i, j].Title = string.Format("{0} of {1}", GCNaksatra.GetPadaText(j), rateNaksatra[i].Title);
                    rateNaksatraPada[i, j].Key = "NaksatraPada";
                }
            }

            for (i = 0; i < 7; i++)
            {
                weekday[i] = new GCConfigRatedEntry();
                weekday[i].Title = string.Format("Weekday: {0}", GCCalendar.GetWeekdayName(i));
                weekday[i].Key = "Weekday";
            }

            for (i = 0; i < 10; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    rateGrahaRasi[i, j] = new GCConfigRatedEntry();
                    rateGrahaRasi[i, j].Title = string.Format("{0} in {1}", GCStrings.GetPlanetNameEn(i), GCRasi.GetNameEn(j));
                    rateGrahaRasi[i, j].Key = "RasiGraha." + GCStrings.GetPlanetNameEn(i);
                    rateGrahaHouse[i, j] = new GCConfigRatedEntry();
                    rateGrahaHouse[i, j].Title = string.Format("{0} in house {1}", GCStrings.GetPlanetNameEn(i), j + 1);
                    rateGrahaHouse[i, j].Key = "HouseGraha." + GCStrings.GetPlanetNameEn(i);

                    rateRasiGraha[j, i] = rateGrahaRasi[i, j];
                    rateHouseGraha[j, i] = rateGrahaHouse[i, j];
                }
            }

            FileName = null;
        }

        /// <summary>
        /// Save configuration into file with given name
        /// </summary>
        /// <param name="fileName">Full path to file</param>
        public void Save(string fileName)
        {
            FileName = fileName;
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("3 {0}|{1}", useAcceptLimit, acceptLimit);
                sw.WriteLine("4 {0}|{1}", useMinPeriodLength, minPeriodLength);
                sw.WriteLine("5 {0}", GCFestivalBase.StringToSafe(Title));
                sw.WriteLine("6 {0}", GCFestivalBase.StringToSafe(Description));

                List<ParameterDescriptor> ar = this.ParameterDescriptions;
                foreach (ParameterDescriptor a in ar)
                {
                    if (a.Array != null)
                    {
                        for (int i = a.Min; i < a.Max; i++)
                        {
                            sw.WriteLine("1 {0}|{1}|{2}|{3}", a.Key, i, a.Array[i].Rating, 
                                GCFestivalBase.StringToSafe(a.Array[i].Note));
                            for (int k = 0; k < a.Array[i].MarginsCount; k++)
                            {
                                GCConfigRatedMargin m = a.Array[i].Margins[k];
                                sw.WriteLine("99 {0}|{1}|{2}|{3}|{4}", GCFestivalBase.StringToSafe(m.Title), m.Rating, 
                                    m.OffsetMinutesStart, m.OffsetMinutesEnd, GCFestivalBase.StringToSafe(m.Note));
                            }
                            a.Array[i].Modified = false;
                        }
                    }
                    else if (a.Array2 != null)
                    {
                        for (int i = a.Min; i < a.Max; i++)
                        {
                            for (int j = a.Min2; j < a.Max2; j++)
                            {
                                sw.WriteLine("2 {0}|{1}|{2}|{3}|{4}", a.Key, i, j, a.Array2[i,j].Rating,
                                    GCFestivalBase.StringToSafe(a.Array2[i,j].Note));
                                for (int k = 0; k < a.Array2[i,j].MarginsCount; k++)
                                {
                                    GCConfigRatedMargin m = a.Array[i].Margins[k];
                                    sw.WriteLine("99 {0}|{1}|{2}|{3}|{4}", GCFestivalBase.StringToSafe(m.Title), m.Rating,
                                        m.OffsetMinutesStart, m.OffsetMinutesEnd, GCFestivalBase.StringToSafe(m.Note));
                                }
                                a.Array2[i, j].Modified = false;
                            }
                        }
                    }
                }

            }
        }

        public ParameterDescriptor GetArrayRef(List<ParameterDescriptor> ar, string key)
        {
            foreach (ParameterDescriptor a in ar)
            {
                if (a.Key.Equals(key))
                    return a;
            }
            return null;
        }
        
        public string FileName { get; set; }

        /// <summary>
        /// Loading configuration from file
        /// </summary>
        /// <param name="fileName"></param>
        public void Load(string fileName)
        {
            FileName = fileName;

            using (StreamReader sr = new StreamReader(fileName))
            {
                List<ParameterDescriptor> ar = this.ParameterDescriptions;
                ParameterDescriptor a;
                GCConfigRatedEntry lastEntry = null;
                int idx, idx2;
                GCRichFileLine rf = new GCRichFileLine();
                while (rf.SetLine(sr.ReadLine()))
                {
                    a = GetArrayRef(ar, rf.GetField(0));
                    try
                    {
                        if (rf.TagInt == 1)
                        {
                            idx = rf.GetFieldInt(1);
                            a.Array[idx].Rating = rf.GetFieldDouble(2);
                            a.Array[idx].Note = rf.GetField(3);
                            lastEntry = a.Array[idx];
                        }
                        else if (rf.TagInt == 2)
                        {
                            idx = rf.GetFieldInt(1);
                            idx2 = rf.GetFieldInt(2);
                            a.Array2[idx, idx2].Rating = rf.GetFieldDouble(3);
                            a.Array2[idx, idx2].Note = rf.GetField(4);
                            lastEntry = a.Array2[idx, idx2];
                        }
                        else if (rf.TagInt == 3)
                        {
                            useAcceptLimit = bool.Parse(rf.GetField(0));
                            acceptLimit = rf.GetFieldDouble(1);
                        }
                        else if (rf.TagInt == 4)
                        {
                            useMinPeriodLength = bool.Parse(rf.GetField(0));
                            minPeriodLength = rf.GetFieldDouble(1);
                        }
                        else if (rf.TagInt == 5)
                        {
                            Title = GCFestivalBase.SafeToString(rf.GetField(0));
                        }
                        else if (rf.TagInt == 6)
                        {
                            Description = GCFestivalBase.SafeToString(rf.GetField(0));
                        }
                        else if (rf.TagInt == 99 && lastEntry != null)
                        {
                            GCConfigRatedMargin m = new GCConfigRatedMargin();
                            m.Title = GCFestivalBase.SafeToString(rf.GetField(0));
                            m.Rating = rf.GetFieldDouble(1);
                            m.OffsetMinutesStart = rf.GetFieldInt(2);
                            m.OffsetMinutesEnd = rf.GetFieldInt(3);
                            m.Note = GCFestivalBase.SafeToString(rf.GetField(4));
                            lastEntry.AddMargin(m);
                        }
                    }
                    catch
                    {
                    }
                }

            }
        }

        public bool RequiredTithi()
        {
            foreach (GCConfigRatedEntry e in rateTithi)
            {
                if (e.Usable)
                    return true;
            }
            return false;
        }

        public bool RequiredNaksatra()
        {
            foreach (GCConfigRatedEntry e in rateNaksatra)
            {
                if (e.Usable)
                    return true;
            }
            for (int i = 0; i < 27; i++)
            {
                if (rateNaksatra[i].Usable)
                    return true;
                for (int j = 0; j < 4; j++)
                {
                    if (rateNaksatraPada[i, j].Usable)
                        return true;
                }
            }
            return false;
        }

        public bool RequiredYoga()
        {
            foreach (GCConfigRatedEntry e in rateYoga)
            {
                if (e.Usable)
                    return true;
            }
            return false;
        }

        public bool RequiredGrahaHouse(int graha)
        {
            for (int i = 0; i < 12; i++)
            {
                if (rateGrahaHouse[graha, i].Usable)
                    return true;
            }
            return false;
        }

        public bool RequiredGrahaRasi(int graha)
        {
            for (int i = 0; i < 12; i++)
            {
                if (rateGrahaRasi[graha, i].Usable)
                    return true;
            }
            return false;
        }

        public bool RequiredMoonTimes()
        {
            return rateMoonTime[0].Usable || rateMoonTime[1].Usable;
        }

        public bool RequiredMuhurta()
        {
            foreach (GCConfigRatedEntry e in rateMuhurta)
            {
                if (e.Usable) return true;
            }
            return false;
        }

        public List<ParameterDescriptor> ParameterDescriptions
        {
            get
            {
                if (p_paramdesc == null)
                {
                    p_paramdesc = new List<ParameterDescriptor>();
                    p_paramdesc.Add(new ParameterDescriptor() { Key = "Tithi", Max = 30, Array = rateTithi, 
                        param1Func = new getParamStringDlg(GCTithi.GetName) });
                    p_paramdesc.Add(new ParameterDescriptor() { Key = "DayHours", Max = 4, Array = rateDayHours, 
                        param1Func = new getParamStringDlg(GCStrings.GetSandhyaName) });
                    p_paramdesc.Add(new ParameterDescriptor() { Key = "DayParts", Max = 2, Array = rateDay,
                        param1Func = new getParamStringDlg(GCStrings.GetDayPartName)});
                    p_paramdesc.Add(new ParameterDescriptor() { Key = "Naksatra", Max = 27, Array = rateNaksatra,
                        param1Func = new getParamStringDlg(GCNaksatra.GetName) });
                    p_paramdesc.Add(new ParameterDescriptor() { Key = "NaksatraPada", Max = 27, Max2 = 4, Array2 = rateNaksatraPada, 
                        param1Func = new getParamStringDlg(GCNaksatra.GetName), 
                        param2Func = new getParamStringDlg(GCNaksatra.GetPadaText) });
                    p_paramdesc.Add(new ParameterDescriptor() { Key = "Yoga", Max = 27, Array = rateYoga, 
                        param1Func = new getParamStringDlg(GCYoga.GetName) });
                    p_paramdesc.Add(new ParameterDescriptor() { Key = "Muhurta", Max = 30, Array = rateMuhurta, 
                        param1Func = new getParamStringDlg(GCStrings.GetMuhurtaName) });
                    p_paramdesc.Add(new ParameterDescriptor() { Key = "Kala", Min = 1, Max = 6, Array = rateKalas, 
                        param1Func = new getParamStringDlg(GCStrings.GetKalaName) });
                    p_paramdesc.Add(new ParameterDescriptor() { Key = "MoonTimes", Max = 2, Array = rateMoonTime, 
                        param1Func = new getParamStringDlg(GCStrings.GetMoonTimesName) });
                    p_paramdesc.Add(new ParameterDescriptor() { Key = "Weekday", Max = 7, Array = weekday, 
                        param1Func = new getParamStringDlg(GCCalendar.GetWeekdayName) });
                    p_paramdesc.Add(new ParameterDescriptor()
                    {
                        Key = "GrahaRasi",
                        Max = 10,
                        Max2 = 12,
                        Array2 = rateGrahaRasi,
                        param1Func = new getParamStringDlg(GCStrings.GetPlanetNameEn),
                        param2Func = new getParamStringDlg(GCRasi.GetNameEn)
                    });
                    p_paramdesc.Add(new ParameterDescriptor()
                    {
                        Key = "GrahaHouse",
                        Max = 10,
                        Max2 = 12,
                        Array2 = rateGrahaHouse,
                        param1Func = new getParamStringDlg(GCStrings.GetPlanetNameEn),
                        param2Func = new getParamStringDlg(GCStrings.GetStringFromIntInc)
                    });
                    p_paramdesc.Add(new ParameterDescriptor()
                    {
                        Key = "RasiGraha",
                        Max = 12,
                        Max2 = 10,
                        Array2 = rateRasiGraha,
                        param2Func = new getParamStringDlg(GCStrings.GetPlanetNameEn),
                        param1Func = new getParamStringDlg(GCRasi.GetNameEn),
                        Additional = true
                    });
                    p_paramdesc.Add(new ParameterDescriptor()
                    {
                        Key = "HouseGraha",
                        Max = 12,
                        Max2 = 10,
                        Array2 = rateHouseGraha,
                        param2Func = new getParamStringDlg(GCStrings.GetPlanetNameEn),
                        param1Func = new getParamStringDlg(GCStrings.GetStringFromIntInc),
                        Additional = true
                    });
                }
                return p_paramdesc;
            }
        }

        public void CopyFrom(GCConfigRatedEvents srcConf)
        {
            List<ParameterDescriptor> target = this.ParameterDescriptions;
            List<ParameterDescriptor> source = srcConf.ParameterDescriptions;

            for(int index = 0; index < target.Count; index++)
            {
                ParameterDescriptor a = target[index];
                ParameterDescriptor b = source[index];
                if (a.Array != null)
                {
                    for (int i = a.Min; i < a.Max; i++)
                    {
                        a.Array[i].Rating = b.Array[i].Rating;
                        a.Array[i].Note = b.Array[i].Note;
                        a.Array[i].Margins = b.Array[i].CloneMargins();
                    }
                }
                else if (a.Array2 != null)
                {
                    for (int i = a.Min; i < a.Max; i++)
                    {
                        for (int j = a.Min2; j < a.Max2; j++)
                        {
                            a.Array2[i,j].Rating = b.Array2[i,j].Rating;
                            a.Array2[i,j].Note = b.Array2[i,j].Note;
                            a.Array2[i,j].Margins = b.Array2[i,j].CloneMargins();
                        }
                    }
                }
            }
        }


        public double GetMaximum()
        {
            List<GCConfigRatedEvents.ParameterDescriptor> pars = ParameterDescriptions;
            double max = 0.0;

            foreach (GCConfigRatedEvents.ParameterDescriptor p in pars)
            {
                double tmp = 0.0;
                if (p.Array != null && !p.Additional)
                {
                    for (int i = p.Min; i < p.Max; i++)
                    {
                        tmp = Math.Max(tmp, p.Array[i].Rating);
                    }
                }
                else if (p.Array2 != null && !p.Additional)
                {
                    for (int i = p.Min; i < p.Max; i++)
                    {
                        for (int j = p.Min2; j < p.Max2; j++)
                        {
                            tmp = Math.Max(tmp, p.Array2[i, j].Rating);
                        }
                    }
                }
                max += Math.Max(0, tmp);
            }

            return max;
        }
    }

    public class GCConfigRatedEntry: GCConfigRateBase
    {
        public bool Modified = false;
        public string Key = null;
        public List<GCConfigRatedMargin> Margins = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title">Name of the interval</param>
        /// <param name="offStart">Offset of the start (in seconds)</param>
        /// <param name="offEnd">Offset of the end (in seconds)</param>
        /// <param name="rating">Rating of this interval</param>
        public void AddMargin(string title, int offStart, int offEnd, double rating)
        {
            GCConfigRatedMargin m = new GCConfigRatedMargin();
            m.Title = title;
            m.Rating = rating;
            m.OffsetMinutesStart = offStart;
            m.OffsetMinutesEnd = offEnd;

            AddMargin(m);
        }

        public void AddMargin(GCConfigRatedMargin m)
        {
            if (Margins == null)
                Margins = new List<GCConfigRatedMargin>();

            Margins.Add(m);
            Modified = true;
        }

        public int MarginsCount
        {
            get
            {
                return Margins == null ? 0 : Margins.Count;
            }
        }

        public GCConfigRatedMargin this[int i]
        {
            get
            {
                return Margins[i];
            }
        }

        public List<GCConfigRatedMargin> CloneMargins()
        {
            if (Margins == null) return null;
            List<GCConfigRatedMargin> list = new List<GCConfigRatedMargin>();
            foreach (GCConfigRatedMargin sm in Margins)
            {
                GCConfigRatedMargin target = new GCConfigRatedMargin();
                target.Note = sm.Note;
                target.OffsetMinutesEnd = sm.OffsetMinutesEnd;
                target.OffsetMinutesStart = sm.OffsetMinutesStart;
                target.Rating = sm.Rating;
                target.Title = sm.Title;
                list.Add(target);
            }

            return list;
        }

        public void DeleteMargin(GCConfigRatedMargin SelectedSubevent)
        {
            if (Margins == null)
                return;
            int i = Margins.IndexOf(SelectedSubevent);
            if (i >= 0 && i < MarginsCount)
            {
                Margins.RemoveAt(i);
                Modified = true;
            }
        }

        public string EncodedStringBase
        {
            get
            {
                return string.Format("{0}|{1}|{2}|{3}", GCFestivalBase.StringToSafe(Title), Rating,
                    Key, GCFestivalBase.StringToSafe(Note));
            }
            set
            {
                string[] p = value.Split('|');
                if (p.Length == 4)
                {
                    Title = GCFestivalBase.SafeToString(p[0]);
                    Rating = double.Parse(p[1]);
                    Key = p[2];
                    Note = GCFestivalBase.SafeToString(p[3]);
                }
            }
        }

        public string EncodedString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(GCFestivalBase.StringToSafe(EncodedStringBase));
                for (int i = 0; i < MarginsCount; i++)
                {
                    if (sb.Length > 0)
                        sb.Append("|");
                    sb.Append(GCFestivalBase.StringToSafe(Margins[i].EncodedString));
                }
                return sb.ToString();
            }
            set
            {
                string[] p = value.Split('|');
                if (p.Length > 0)
                    EncodedStringBase = GCFestivalBase.SafeToString(p[0]);
                Margins = null;
                for (int i = 1; i < p.Length; i++)
                {
                    GCConfigRatedMargin m = new GCConfigRatedMargin();
                    m.EncodedString = GCFestivalBase.SafeToString(p[i]);
                    AddMargin(m);
                }
                Modified = false;
            }
        }
    }

    public class GCConfigRatedMargin: GCConfigRateBase
    {
        public int OffsetMinutesStart = 0;
        public int OffsetMinutesEnd = 0;

        public string EncodedString
        {
            get
            {
                return string.Format("{0}|{1}|{2}|{3}|{4}", GCFestivalBase.StringToSafe(Title), Rating, 
                    OffsetMinutesStart, OffsetMinutesEnd, GCFestivalBase.StringToSafe(Note));
            }
            set
            {
                string[] p = value.Split('|');
                if (p.Length == 5)
                {
                    Title = GCFestivalBase.SafeToString(p[0]);
                    Rating = double.Parse(p[1]);
                    OffsetMinutesStart = int.Parse(p[2]);
                    OffsetMinutesEnd = int.Parse(p[3]);
                    Note = GCFestivalBase.SafeToString(p[4]);
                }
            }
        }
    }

    public class GCConfigRateBase
    {
        public string Title = string.Empty;
        public double Rating = 0.0;
        public string Note = null;

        public bool HasNote
        {
            get
            {
                return Note != null && Note.Length > 0;
            }
        }

        /// <summary>
        /// Returns flag, if this entry needs to be calculated and used for rating
        /// </summary>
        public bool Usable
        {
            get
            {
                return Rating != 0.0;
            }
        }
    }

    public class GCRatedMoment
    {
        public class ComparerClass : IComparer<GCRatedMoment>
        {

            // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            public int Compare(GCRatedMoment A, GCRatedMoment B)
            {
                return GregorianDateTime.Compare(A.JulianDay, B.JulianDay);
            }

        }

        public GregorianDateTime JulianDay = null;
        public string Title;
        public string Note;
        public double Rating;
        //public GCConfigRateBase Entry = null;
        public string Key = null;

        public override string ToString()
        {
            return string.Format("{0} {1} :: {2} [{3}]", JulianDay.ToString(), JulianDay.LongTimeString(), Title, Rating);
        }
    }
}
