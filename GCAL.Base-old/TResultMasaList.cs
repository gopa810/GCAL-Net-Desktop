using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public class TResultMasaList: TResultBase
    {
        public List<TResultMasa> arr = new List<TResultMasa>();
        public GregorianDateTime vc_end;
        public GregorianDateTime vc_start;
        public int n_countYears;
        public int n_countMasa;
        public int n_startYear;
        public GCLocation m_location;


        public TResultMasaList()
        {
            n_countMasa = 0;
            n_countYears = 0;
            n_startYear = 0;
            arr.Clear();
        }

        public override GSCore GetPropertyValue(string s)
        {
            switch (s)
            {
                case "items":
                    GSList list = new GSList();
                    foreach (TResultMasa m in arr)
                        list.Add(m);
                    return list;
                case "startDate":
                    return vc_start;
                case "endDate":
                    return vc_end;
                case "countYears":
                    return new GSNumber(n_countYears);
                case "countMasa":
                    return new GSNumber(n_countMasa);
                case "startYear":
                    return new GSNumber(n_startYear);
                case "location":
                    return m_location;
                default:
                    break;
            }
            return base.GetPropertyValue(s);
        }

        /// <summary>
        /// Calculation of Masa List
        /// </summary>
        /// <param name="loc">Location</param>
        /// <param name="nYear">Starting year</param>
        /// <param name="nCount">Number of years</param>
        /// <returns></returns>
        public int CalculateMasaList(GCLocation loc, int nYear, int nCount)
        {
            GCAstroData day = new GCAstroData();
            GregorianDateTime d = new GregorianDateTime(), de = new GregorianDateTime(), t = new GregorianDateTime();
            int lm = -1;
            TResultMasaList mlist = this;
            GCEarthData earth = loc.GetEarthData();

            mlist.n_startYear = nYear;
            mlist.n_countYears = nCount;
            mlist.vc_start = new GregorianDateTime();
            mlist.vc_end = new GregorianDateTime();
            mlist.vc_start.Set(GCAstroData.GetFirstDayOfYear(earth, nYear));
            mlist.vc_end.Set(GCAstroData.GetFirstDayOfYear(earth, nYear + nCount));
            mlist.m_location = loc;

            d.Set(mlist.vc_start);
            de.Set(mlist.vc_end);

            int i = 0;
            int prev_paksa = -1;
            int current = 0;


            while (d.IsBeforeThis(de))
            {
                day.DayCalc(d, earth);
                if (prev_paksa != day.sunRise.Paksa)
                {
                    day.Masa = day.MasaCalc(d, earth);

                    if (lm != day.Masa)
                    {
                        if (lm >= 0)
                        {
                            t.Set(d);
                            t.PreviousDay();
                            if (mlist.arr.Count <= current)
                                mlist.arr.Add(new TResultMasa());
                            mlist.arr[current].vc_end = new GregorianDateTime(t);
                            current++;
                        }
                        lm = day.Masa;
                        if (mlist.arr.Count <= current)
                            mlist.arr.Add(new TResultMasa());
                        mlist.arr[current].masa = day.Masa;
                        mlist.arr[current].year = day.GaurabdaYear;
                        mlist.arr[current].vc_start = new GregorianDateTime(d);
                    }
                }
                prev_paksa = day.sunRise.Paksa;
                d.NextDay();
                i++;
            }

            mlist.arr[current].vc_end = new GregorianDateTime(d);
            current++;
            mlist.n_countMasa = current;

            return 1;
        }

        /// <summary>
        /// Generates text file based on given data format template
        /// </summary>
        /// <param name="str">Output stream</param>
        /// <param name="df">Requested data format</param>
        /// <returns></returns>
        public override string formatText(string df)
        {
            GSScript script = new GSScript();
            switch (df)
            {
                case GCDataFormat.PlainText:
                    script.readTextTemplate(Properties.Resources.TplMasaListPlain);
                    break;
                case GCDataFormat.Rtf:
                    script.readTextTemplate(Properties.Resources.TplMasaListRtf);
                    break;
                case GCDataFormat.HTML:
                    script.readTextTemplate(Properties.Resources.TplMasaListHtml);
                    break;
                case GCDataFormat.XML:
                    script.readTextTemplate(Properties.Resources.TplMasaListXml);
                    break;
                case GCDataFormat.CSV:
                    script.readTextTemplate(Properties.Resources.TplMasaListCsv);
                    break;
                default:
                    break;
            }


            GSExecutor engine = new GSExecutor();
            engine.SetVariable("mlist", this);
            engine.SetVariable("location", this.m_location);
            engine.SetVariable("app", GCUserInterface.Shared);
            engine.ExecuteElement(script);

            return engine.getOutput();
        }

        public override TResultFormatCollection getFormats()
        {
            TResultFormatCollection coll = base.getFormats();

            coll.ResultName = "MasaList";
            coll.Formats.Add(new TResultFormat("Text File", "txt", GCDataFormat.PlainText));
            coll.Formats.Add(new TResultFormat("Rich Text File", "rtf", GCDataFormat.Rtf));
            coll.Formats.Add(new TResultFormat("XML File", "xml", GCDataFormat.XML));
            coll.Formats.Add(new TResultFormat("Comma Separated Values", "csv", GCDataFormat.CSV));
            coll.Formats.Add(new TResultFormat("HTML File (in List format)", "htm", GCDataFormat.HTML));
            return coll;
        }

    }

    public class TResultMasa: GSCore
	{
		public int masa;
		public int year;
		public GregorianDateTime vc_start;
		public GregorianDateTime vc_end;

        public override GSCore GetPropertyValue(string s)
        {
            switch (s)
            {
                case "masa":
                    return new GSNumber(masa);
                case "masaName":
                    return new GSString(GCMasa.GetName(masa));
                case "year":
                    return new GSNumber(year);
                case "startDate":
                    return vc_start;
                case "endDate":
                    return vc_end;
                default:
                    break;
            }
            return base.GetPropertyValue(s);
        }

	};

}
