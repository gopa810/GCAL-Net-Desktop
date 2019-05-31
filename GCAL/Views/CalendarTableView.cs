using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GCAL.Base;

namespace GCAL.Views
{
    public partial class CalendarTableView : UserControl
    {
        private int currentYear = 0;
        private int currentMonth = 0;
        private TResultCalendar calendar = null;
        private bool liveRefresh = false;
        private CalendarTableDrawer drawer = new CalendarTableDrawer();
        public GCLocation calLocation = null;

        public bool LiveRefresh
        {
            get { return liveRefresh; }
            set 
            {
                liveRefresh = value;
                if (value)
                    RecalculateCalendar();
            }
        }

        public int CurrentYear
        {
            get
            {
                return currentYear;
            }
            set
            {
                if (currentYear != value)
                {
                    currentYear = value;
                    if (liveRefresh)
                        RecalculateCalendar();
                }
            }
        }

        public int CurrentMonth
        {
            get
            {
                return currentMonth;
            }
            set
            {
                if (currentMonth != value)
                {
                    currentMonth = value;
                    if (liveRefresh)
                        RecalculateCalendar();
                }
            }
        }

        public void RecalculateCalendar()
        {
            calendar = new TResultCalendar();
            calendar.CalculateCalendar(calLocation, new GregorianDateTime(currentYear, currentMonth, 1), 31);
            Invalidate();
        }

        public TResultCalendar SelectedCalendar
        {
            get
            {
                return calendar;
            }
            set
            {
                calendar = null;
            }
        }

        public string GetTitleString()
        {
            return string.Format("{0} {1}", GregorianDateTime.GetMonthName(currentMonth), currentYear);
        }


        public CalendarTableView()
        {
            InitializeComponent();

            DateTime dt = DateTime.Now;

            currentYear = dt.Year;
            currentMonth = dt.Month;

            drawer.ShowTodayBorder = true;
        }

        private void CalendarTableView_Paint(object sender, PaintEventArgs e)
        {
            drawer.Draw(e.Graphics, this.Size, this.calendar, currentYear, currentMonth);
        }

        private void CalendarTableView_SizeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
