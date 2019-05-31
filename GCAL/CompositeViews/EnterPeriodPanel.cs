using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GCAL.Base;
using GCAL.Views;
using GCAL.Base.Scripting;

namespace GCAL.CompositeViews
{
    public partial class EnterPeriodPanel : UserControl
    {
        public class DateRecord
        {
            public string Title = "";
            public string Subtitle = "";
            public GregorianDateTime StartDate = null;
            public GregorianDateTime EndDate = null;
            public void UpdateSubtitle()
            {
                Subtitle = string.Format("Period: {0} - {1}", StartDate.ToString(), EndDate.ToString());
            }
        }

        public EnterPeriodPanelController Controller { get; set; }

        public event TBButtonPressed OnPeriodSelected;

        public GregorianDateTime InputStartDate { get; set; }
        public GregorianDateTime InputEndDate { get; set; }
        public GCEarthData EarthLocation { get; set; }
        public int PaddingX = 8;
        public int Spacing = 4;
        private Font CaptionFont = null;
        private Font TextFont = null;

        public EnterPeriodPanel()
        {
            InitializeComponent();
            CaptionFont = new Font(FontFamily.GenericSansSerif, 13);
            TextFont = new Font(FontFamily.GenericSansSerif, 10);
            DateRecord dr;

            DateTime d = DateTime.Now;

            dr = new DateRecord(){Title = "Year " + d.Year};
            dr.StartDate = new GregorianDateTime(DateTime.Now.Year, 1, 1);
            dr.EndDate = new GregorianDateTime(DateTime.Now.Year, 12, 31);
            dr.UpdateSubtitle();
            listBox1.Items.Add(dr);

            dr = new DateRecord() { Title = "Year " + (d.Year+1).ToString()};
            dr.StartDate = new GregorianDateTime(DateTime.Now.Year + 1, 1, 1);
            dr.EndDate = new GregorianDateTime(DateTime.Now.Year + 1, 12, 31);
            dr.UpdateSubtitle();
            listBox1.Items.Add(dr);

            dr = new DateRecord();
            if (d.Month == 12)
            {
                dr.Title = string.Format("{0} {1} - {2} {3}", GregorianDateTime.GetMonthAbreviation(12), d.Year,
                    GregorianDateTime.GetMonthAbreviation(1), d.Year + 1);
                dr.StartDate = new GregorianDateTime(d.Year, 12, 1);
                dr.EndDate = new GregorianDateTime(d.Year + 1, 1, 31);
            }
            else
            {
                dr.Title = string.Format("{0} - {1} {2}", GregorianDateTime.GetMonthAbreviation(d.Month),
                    GregorianDateTime.GetMonthAbreviation(d.Month + 1), d.Year);
                dr.StartDate = new GregorianDateTime(d.Year, d.Month, 1);
                dr.EndDate = new GregorianDateTime(d.Year, d.Month + 1, GregorianDateTime.GetMonthMaxDays(d.Year, d.Month + 1));
            }
            dr.UpdateSubtitle();
            listBox1.Items.Add(dr);

            dr = new DateRecord();
            dr.Title = "Custom Date Range";
            dr.Subtitle = "Enter start date manually";
            listBox1.Items.Add(dr);
        }

        private void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            SizeF a = e.Graphics.MeasureString("A", CaptionFont);
            SizeF b = e.Graphics.MeasureString("B", TextFont);

            e.ItemHeight = 2 * PaddingX + Spacing + Convert.ToInt32(a.Height + b.Height);
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0 && e.Index < listBox1.Items.Count)
            {
                DateRecord cr = listBox1.Items[e.Index] as DateRecord;

                if ((e.State & DrawItemState.Selected) != 0)
                {
                    e.Graphics.FillRectangle(Brushes.LightYellow, e.Bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(SystemBrushes.Control, e.Bounds);
                }

                SizeF a = e.Graphics.MeasureString("A", CaptionFont);
                e.Graphics.DrawString(cr.Title, CaptionFont, Brushes.Black, PaddingX, e.Bounds.Top + PaddingX);
                e.Graphics.DrawString(cr.Subtitle, TextFont, Brushes.Gray, PaddingX, e.Bounds.Top + PaddingX + a.Height + Spacing);
            }

        }

        public GregorianDateTime StartDate
        {
            get
            {
                int iu = listBox1.SelectedIndex;
                if (iu >= 0 && iu < listBox1.Items.Count)
                {
                    DateRecord dr = (DateRecord)listBox1.Items[iu];
                    return dr.StartDate;
                }

                return null;
            }
        }

        public GregorianDateTime EndDate
        {
            get
            {
                int iu = listBox1.SelectedIndex;
                if (iu >= 0 && iu < listBox1.Items.Count)
                {
                    DateRecord dr = (DateRecord)listBox1.Items[iu];
                    return dr.EndDate;
                }

                return null;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (Controller != null)
            {
                GregorianDateTime dt = StartDate;
                if (dt != null)
                {
                    InputStartDate = StartDate;
                    InputEndDate = EndDate;
                    if (OnPeriodSelected != null)
                        OnPeriodSelected(this, e);
                }
                else
                {
                    StartDatePanel d = new StartDatePanel();
                    d.CustomTitle = "Enter Start Date";
                    d.EarthLocation = this.EarthLocation;
                    d.GregorianTime = InputStartDate;
                    d.OnStartDateDone += new TBButtonPressed(d_OnStartDateDone);
                    StartDatePanelController dc = new StartDatePanelController(d);
                    dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Center);
                }
            }

            Controller.RemoveFromContainer();
        }

        private void d_OnStartDateDone(object sender, EventArgs e)
        {
            if (sender is GregorianDateTime)
            {
                InputStartDate = sender as GregorianDateTime;

                // display dialog for end date
                StartDatePanel d = new StartDatePanel();
                d.CustomTitle = "Enter End Date";
                d.EarthLocation = this.EarthLocation;
                d.GregorianTime = InputEndDate;
                d.OnStartDateDone += new TBButtonPressed(d_OnEndDateDone);
                StartDatePanelController dc = new StartDatePanelController(d);
                dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Center);
            }
        }

        private void d_OnEndDateDone(object sender, EventArgs e)
        {
            if (sender is GregorianDateTime)
            {
                InputEndDate = sender as GregorianDateTime;

                if (InputEndDate.GetJulianInteger() < InputStartDate.GetJulianInteger() + 7)
                {
                    InputEndDate = new GregorianDateTime(InputStartDate);
                    InputEndDate.AddDays(7);
                }

                if (OnPeriodSelected != null)
                    OnPeriodSelected(this, e);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Controller.RemoveFromContainer();
        }
    }


    public class EnterPeriodPanelController : GVCore
    {
        public EnterPeriodPanelController(EnterPeriodPanel v)
        {
            View = v;
            v.Controller = this;
        }
    }
}
