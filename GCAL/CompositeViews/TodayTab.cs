using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using GCAL.Base;
using GCAL.Base.Scripting;
using GCAL.Views;
using GCAL.CompositeViews;

namespace GCAL.CompositeViews
{
    public partial class TodayTab : UserControl
    {
        private TResultToday m_today;
        public TodayTabController Controller { get; set; }
        private int p_mode = 0;
        public GregorianDateTime selectedDate = null;

        public TodayTab()
        {
            InitializeComponent();
            richTextBox1.Dock = DockStyle.Fill;
            pictureBox1.Dock = DockStyle.Fill;

            selectedDate = new GregorianDateTime();
            LocationText(GCGlobal.myLocation.Title);
            StartDateText(selectedDate.ToString());
            SetMode(Properties.Settings.Default.TodayShowMode);
        }

        public TResultBase getCurrentContent()
        {
            return m_today;
        }

        public void SetMode(int i)
        {
            if (i == -1)
            {
                richTextBox1.Visible = false;
                pictureBox1.Visible = true;
            }
            else if (i == 0)
            {
                richTextBox1.Visible = true;
                pictureBox1.Visible = false;

                p_mode = i;
                DisplayCalendarResult();
                Properties.Settings.Default.TodayShowMode = i;
                Properties.Settings.Default.Save();
            }
            else if (i == 1)
            {
                richTextBox1.Visible = true;
                pictureBox1.Visible = false;

                p_mode = i;
                DisplayCalendarResult();
                Properties.Settings.Default.TodayShowMode = i;
                Properties.Settings.Default.Save();
            }

        }

        public string LocationText(string s)
        {
            if (s != null)
            {
                toolStripButton1.Text = s;
            }
            return toolStripButton1.Text;
        }

        public string StartDateText(string s)
        {
            if (s != null)
            {
                toolStripButton2.Text = s;
            }
            return toolStripButton2.Text;
        }

        public GregorianDateTime SelectedDateTime
        {
            get
            {
                return selectedDate;
            }
        }

        public void Recalculate()
        {
            if (p_mode == 0 || p_mode == 1)
            {
                m_today = new TResultToday();
                m_today.Calculate(SelectedDateTime, GCGlobal.myLocation);
                DisplayCalendarResult();
            }
        }

        public void DisplayCalendarResult()
        {
            if (p_mode == 0)
            {
                if (m_today == null)
                    Recalculate();
                else
                    richTextBox1.Text = m_today.formatText(GCDataFormat.PlainText);
            }
            else if (p_mode == 1)
            {
                if (m_today == null)
                    Recalculate();
                else
                    richTextBox1.Rtf = m_today.formatText(GCDataFormat.Rtf);
            }
        }

        private void onLocationClick(object sender, EventArgs e)
        {
            SelectLocationInputPanel d = new SelectLocationInputPanel();
            d.OnLocationSelected += new TBButtonPressed(onLocationDone);
            d.HideMyLocation();
            SelectLocationInputPanelController dc = new SelectLocationInputPanelController(d);
            dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Center);
        }

        private void onLocationDone(object sender, EventArgs e)
        {
            if (sender is GCLocation)
            {
                GCLocation lr = sender as GCLocation;
                GCGlobal.myLocation = lr;
                LocationText(lr.Title);
                Recalculate();
            }
        }

        private void onDateRangeClick(object sender, EventArgs e)
        {
            StartDatePanel d = new StartDatePanel();
            d.CustomTitle = "Enter Date";
            d.EarthLocation = GCGlobal.myLocation.GetEarthData();
            d.OnStartDateDone += new TBButtonPressed(onDateRangeClickDone);
            d.GregorianTime = selectedDate;

            StartDatePanelController dlg13b = new StartDatePanelController(d);
            dlg13b.ShowInContainer(Controller.ViewContainer, GVControlAlign.Center);
        }

        private void onDateRangeClickDone(object sender, EventArgs e)
        {
            if (sender is GregorianDateTime)
            {
                GregorianDateTime gdt = sender as GregorianDateTime;
                selectedDate = gdt;
                StartDateText(gdt.ToString());
                Recalculate();
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            selectedDate = new GregorianDateTime();
            StartDateText(selectedDate.ToString());
            Recalculate();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            selectedDate.SubtractDays(1);
            StartDateText(selectedDate.ToString());
            Recalculate();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            selectedDate.AddDays(1);
            StartDateText(selectedDate.ToString());
            Recalculate();
        }

        private void exportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrameMain.SaveContentPlain(m_today);
        }

        private void printToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (Controller != null)
                Controller.ExecuteMessage("printContent", m_today);
        }

        private void toolStripDropDownButton2_DropDownOpening(object sender, EventArgs e)
        {
            plainTextToolStripMenuItem.Checked = (p_mode == 0);
            richTextToolStripMenuItem.Checked = (p_mode == 1);

            smallTextToolStripMenuItem.Enabled = (p_mode != 2);
            smallTextToolStripMenuItem1.Enabled = (p_mode != 2);
            normalTextToolStripMenuItem.Enabled = (p_mode != 2);
            largestTextToolStripMenuItem.Enabled = (p_mode != 2);
            largeTextToolStripMenuItem.Enabled = (p_mode != 2);

            smallTextToolStripMenuItem.Checked = (GCLayoutData.LayoutSizeIndex == 0);
            smallTextToolStripMenuItem1.Checked = (GCLayoutData.LayoutSizeIndex == 1);
            normalTextToolStripMenuItem.Checked = (GCLayoutData.LayoutSizeIndex == 2);
            largeTextToolStripMenuItem.Checked = (GCLayoutData.LayoutSizeIndex == 3);
            largestTextToolStripMenuItem.Checked = (GCLayoutData.LayoutSizeIndex == 4);

        }

        private void plainTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMode(0);
        }

        private void richTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMode(1);
        }

        private void smallTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GCLayoutData.LayoutSizeIndex = 0;
            DisplayCalendarResult();
        }

        private void smallTextToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GCLayoutData.LayoutSizeIndex = 1;
            DisplayCalendarResult();
        }

        private void normalTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GCLayoutData.LayoutSizeIndex = 2;
            DisplayCalendarResult();
        }

        private void largeTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GCLayoutData.LayoutSizeIndex = 3;
            DisplayCalendarResult();
        }

        private void largestTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GCLayoutData.LayoutSizeIndex = 4;
            DisplayCalendarResult();
        }
    }
}
