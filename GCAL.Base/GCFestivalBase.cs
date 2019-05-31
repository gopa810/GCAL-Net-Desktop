using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public class GCFestivalBase: GSCore
    {
        /// <summary>
        /// Name of person or the reason for fasting
        /// </summary>
        public string FastingSubject;
        /// <summary>
        /// Title of event or festival
        /// </summary>
        public String Text;
        /// <summary>
        /// ID of parent festival book
        /// </summary>
        public int BookID;
        /// <summary>
        /// ID of fasting, see FastType class for values
        /// </summary>
        public int FastID;
        /// <summary>
        /// Unique ID of this event (festival)
        /// </summary>
        public int EventID;
        /// <summary>
        /// 1 - event is shown in calendar, 1 - not shown
        /// </summary>
        public int nVisible;
        /// <summary>
        /// Starting gregorian year of the event
        /// value -10000 means UNDEF
        /// </summary>
        public int StartYear;
        /// <summary>
        /// Not used
        /// </summary>
        public int nReserved;
        /// <summary>
        /// Used only for in-memory, not stored to disk file
        /// </summary>
        public int nDeleted;
        /// <summary>
        /// Offset in days, positive values means this event occurs in the future (relatively to reference data)
        /// </summary>
        public int DayOffset;

        /// <summary>
        /// Related events
        /// </summary>
        private List<GCFestivalBase> p_events = null;

        private static int globalEventId = 1;

        public GCFestivalBase()
        {
            BookID = -1;
            FastID = 0;
            nVisible = 1;
            nReserved = 1;
            nDeleted = 0;
            EventID = globalEventId;
            globalEventId++;
            DayOffset = 0;
            StartYear = -10000;
        }

        public int EventsCount
        {
            get
            {
                if (p_events == null)
                    return 0;
                return p_events.Count;
            }
        }

        public List<GCFestivalBase> Events
        {
            get
            {
                if (p_events == null)
                    return new List<GCFestivalBase>();
                return p_events;
            }
        }

        public virtual void updateListBoxEntry(GCListBoxEntry lb)
        {
            lb.Tag = this;
            lb.Title = Text;
        }

        public string EncodedString
        {
            get
            {
                return getEncoded();
            }
            set
            {
                string[] sa = value.Split('|');
                setEncoded(sa);
            }
        }

        public static string StringToSafe(string s)
        {
            if (s == null)
                return string.Empty;
            return s.Replace("&", "&amp;").Replace("|", "&sep;");
        }

        public static string SafeToString(string s)
        {
            return s.Replace("&amp;", "&").Replace("&sep;", "|");
        }

        public static int StringToInt(string s, int defaultValue)
        {
            int returnValue = defaultValue;
            if (int.TryParse(s, out returnValue))
                return returnValue;
            return defaultValue;
        }

        public static string[] Subarray(string[] sa, int from)
        {
            if (sa.Length <= from)
                return new string[] { };
            string[] st = new string[sa.Length - from];
            Array.Copy(sa, from, st, 0, sa.Length - from);
            return st;
        }

        public virtual GSList getScript()
        {
            GSList list = new GSList();
            list.Add(new GSToken("festival"));
            list.Add(new GSList(new GSToken("text"), new GSString(Text)));
            if (FastingSubject.Length > 0)
                list.Add(new GSList(new GSToken("fastSubject"), new GSString(FastingSubject)));
            list.Add(new GSList(new GSToken("bookId"), new GSNumber(BookID)));
            list.Add(new GSList(new GSToken("fastType"), new GSNumber(FastID)));
            if (nVisible == 0)
                list.Add(new GSList(new GSToken("visible"), new GSNumber(nVisible)));
            if (StartYear > -10000)
                list.Add(new GSList(new GSToken("startYear"), new GSNumber(StartYear)));
            if (nReserved != 1)
                list.Add(new GSList(new GSToken("used"), new GSNumber(nReserved)));
            if (EventID != 0)
                list.Add(new GSList(new GSToken("specId"), new GSNumber(EventID)));
            if (DayOffset != 0)
                list.Add(new GSList(new GSToken("dayOffset"), new GSNumber(DayOffset)));

            return list;
        }

        public override GSCore ExecuteMessage(string token, GSCoreCollection args)
        {
            if (token.Equals("getScript"))
            {
                return getScript();
            }
            return base.ExecuteMessage(token, args);
        }

        protected virtual string getEncoded()
        {
            return string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}",
                StringToSafe(FastingSubject),
                StringToSafe(Text),
                BookID, FastID,
                nVisible, StartYear,
                nReserved, EventID, DayOffset);
        }

        protected virtual string[] setEncoded(string[] sa)
        {
            if (sa.Length < 9)
                return sa;
            FastingSubject = SafeToString(sa[0]);
            Text = SafeToString(sa[1]);
            BookID = StringToInt(sa[2], 6);
            FastID = StringToInt(sa[3], 0);
            nVisible = StringToInt(sa[4], 1);
            StartYear = StringToInt(sa[5], -10000);
            nReserved = StringToInt(sa[6], 1);
            int j = StringToInt(sa[7], 0);
            if (j > 0) EventID = j;
            globalEventId = Math.Max(EventID + 1, globalEventId);
            DayOffset = StringToInt(sa[8], 0);
            return Subarray(sa, 9);
        }

        public virtual string getToken()
        {
            return GetType().Name;
        }

        /// <summary>
        /// Adding new related event
        /// </summary>
        /// <param name="re"></param>
        public void AddRelatedFestival(GCFestivalRelated re)
        {
            if (p_events == null)
                p_events = new List<GCFestivalBase>();
            p_events.Add(re);
        }

        /// <summary>
        /// returns true, if on exec.day(0) should be observed this event
        /// </summary>
        /// <param name="exec"></param>
        /// <returns></returns>
        public virtual bool IsFestivalDay(GCFestivalSpecialExecutor exec)
        {
            return false;
        }

        /// <summary>
        /// Removing related event based on EventID
        /// </summary>
        /// <param name="cFestival"></param>
        public void Remove(GCFestivalBase cFestival)
        {
            if (p_events == null)
                return;
            for (int i = 0; i < p_events.Count; i++)
            {
                if (p_events[i].EventID == cFestival.EventID)
                {
                    p_events.RemoveAt(i);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Generic object for listbox content
    /// </summary>
    public class GCListBoxEntry
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int TitleHeight = -1;
        public int SubtitleHeight = -1;
        public object Tag { set; get; }

        public GCListBoxEntry()
        {
            Title = "";
            Subtitle = "";
        }
    }
}
