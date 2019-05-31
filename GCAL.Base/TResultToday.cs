using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public class TResultToday: TResultBase
    {
        public GregorianDateTime currentDay;
        public TResultCalendar calendar;

        public TResultToday()
        {
            calendar = new TResultCalendar();
        }

        public void Calculate(GregorianDateTime dateTime, GCLocation location)
        {
            GregorianDateTime vc2 = new GregorianDateTime();
            currentDay = new GregorianDateTime();
            currentDay.Set(dateTime);
            currentDay.InitWeekDay();
            vc2.Set(currentDay);

            vc2.TimezoneHours = location.OffsetUtcHours;
            vc2.PreviousDay();
            vc2.PreviousDay();
            vc2.PreviousDay();
            vc2.PreviousDay();
            calendar = new TResultCalendar();
            calendar.CalculateCalendar(location, vc2, 9);

        }


        public VAISNAVADAY GetCurrentDay()
        {
            int i = calendar.FindDate(currentDay);
            return calendar.GetDay(i);
        }

        /// <summary>
        /// Formating of data into text file
        /// </summary>
        /// <param name="str">Output string builder</param>
        /// <param name="df">Requested data format</param>
        public override string formatText(string df)
        {
            GSScript script = new GSScript();
            switch (df)
            {
                case GCDataFormat.PlainText:
                    script.readTextTemplate(Properties.Resources.TplTodayPlain);
                    break;
                case GCDataFormat.Rtf:
                    script.readTextTemplate(Properties.Resources.TplTodayRtf);
                    break;
                case GCDataFormat.HTML:
                    script.readTextTemplate(Properties.Resources.TplTodayHtml);
                    break;
                default:
                    break;
            }

            GSExecutor engine = new GSExecutor();
            engine.SetVariable("today", GetCurrentDay());
            engine.SetVariable("location", calendar.m_Location);
            engine.SetVariable("app", GCUserInterface.Shared);
            engine.ExecuteElement(script);

            return engine.getOutput();
        }

        public override TResultFormatCollection getFormats()
        {
            TResultFormatCollection coll = base.getFormats();

            coll.ResultName = "Today";
            coll.Formats.Add(new TResultFormat("Text File", "txt", GCDataFormat.PlainText));
            coll.Formats.Add(new TResultFormat("Rich Text File", "rtf", GCDataFormat.Rtf));
            coll.Formats.Add(new TResultFormat("XML File", "xml", GCDataFormat.XML));
            coll.Formats.Add(new TResultFormat("Comma Separated Values", "csv", GCDataFormat.CSV));
            coll.Formats.Add(new TResultFormat("HTML File (in List format)", "htm", GCDataFormat.HTML));
            return coll;
        }

    }
}
