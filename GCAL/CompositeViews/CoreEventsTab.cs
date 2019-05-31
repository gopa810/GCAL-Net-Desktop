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

namespace GCAL.CompositeViews
{
    public partial class CoreEventsTab : UserControl
    {
        TResultEvents m_events = null;
        public CoreEventsTabController Controller { get; set; }
        private int p_mode = 0;
        public GCLocation coreLocation = null;
        public GregorianDateTime coreStartDate = new GregorianDateTime();
        public GregorianDateTime coreEndDate = new GregorianDateTime();

        public CoreEventsTab()
        {
            InitializeComponent();
            richTextBox1.Dock = DockStyle.Fill;
            pictureBox1.Dock = DockStyle.Fill;

            string s = Properties.Settings.Default.CoreEventsLocation;
            if (s.Length < 1)
                s = GCGlobal.LastLocation.EncodedString;
            coreLocation = new GCLocation();
            coreLocation.EncodedString = s;
            s = Properties.Settings.Default.CoreEventsStartDate;
            if (s.Length > 1)
                coreStartDate.EncodedString = s;
            s = Properties.Settings.Default.CoreEventsEndDate;
            if (s.Length > 1)
                coreEndDate.EncodedString = s;

            SetMode(Properties.Settings.Default.CoreEventsShowMode);
        }

        public TResultBase getCurrentContent()
        {
            return m_events;
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
                Properties.Settings.Default.CoreEventsShowMode = i;
                Properties.Settings.Default.Save();
            }
            else if (i == 1)
            {
                richTextBox1.Visible = true;
                pictureBox1.Visible = false;

                p_mode = i;
                DisplayCalendarResult();
                Properties.Settings.Default.CoreEventsShowMode = i;
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
        public string EndDateText(string s)
        {
            if (s != null)
            {
                toolStripButton3.Text = s;
            }
            return toolStripButton3.Text;
        }

        public void Recalculate()
        {
            bool settingsChanged = false;
            string s = coreLocation.EncodedString;
            if (!s.Equals(Properties.Settings.Default.CoreEventsLocation))
            {
                Properties.Settings.Default.CoreEventsLocation = s;
                settingsChanged = true;
            }
            s = coreStartDate.EncodedString;
            if (!s.Equals(Properties.Settings.Default.CoreEventsStartDate))
            {
                Properties.Settings.Default.CoreEventsStartDate = s;
                settingsChanged = true;
            }
            s = coreEndDate.EncodedString;
            if (!s.Equals(Properties.Settings.Default.CoreEventsEndDate))
            {
                Properties.Settings.Default.CoreEventsEndDate = s;
                settingsChanged = true;
            }
            if (settingsChanged)
            {
                Properties.Settings.Default.Save();
            }

            if (p_mode == 0 || p_mode == 1)
            {
                LocationText(coreLocation.Title);
                StartDateText(coreStartDate.ToString());
                EndDateText(coreEndDate.ToString());

                m_events = new TResultEvents();
                m_events.CalculateEvents(coreLocation, coreStartDate, coreEndDate);
                DisplayCalendarResult();
            }
        }

        public void DisplayCalendarResult()
        {
            if (p_mode == 0)
            {
                if (m_events == null)
                    Recalculate();
                else
                    richTextBox1.Text = m_events.formatText(GCDataFormat.PlainText);
            }
            else if (p_mode == 1)
            {
                if (m_events == null)
                    Recalculate();
                else
                    richTextBox1.Rtf = m_events.formatText(GCDataFormat.Rtf);
            }
        }

        private void onLocationClick(object sender, EventArgs e)
        {
            SelectLocationInputPanel d = new SelectLocationInputPanel();
            d.OnLocationSelected += new TBButtonPressed(onLocationDone);
            SelectLocationInputPanelController dc = new SelectLocationInputPanelController(d);
            dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Center);
        }

        private void onLocationDone(object sender, EventArgs e)
        {
            if (sender is GCLocation)
            {
                GCLocation lr = sender as GCLocation;
                GCGlobal.AddRecentLocation(lr);
                coreLocation = lr;
                Recalculate();
            }
        }

        private void onDateRangeClick(object sender, EventArgs e)
        {
            EnterPeriodPanel d = new EnterPeriodPanel();
            d.EarthLocation = coreLocation.GetEarthData();
            d.OnPeriodSelected += new TBButtonPressed(d_OnPeriodSelected);
            d.InputStartDate = coreStartDate;
            d.InputEndDate = coreEndDate;
            EnterPeriodPanelController dlg15 = new EnterPeriodPanelController(d);
            dlg15.ShowInContainer(Controller.ViewContainer, GVControlAlign.Center);
        }

        private void d_OnPeriodSelected(object sender, EventArgs e)
        {
            if (sender is EnterPeriodPanel)
            {
                EnterPeriodPanel d = sender as EnterPeriodPanel;
                coreStartDate = d.InputStartDate;
                coreEndDate = d.InputEndDate;
                Recalculate();
            }
        }

        private void exportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrameMain.SaveContentPlain(m_events);
        }

        private void printToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (Controller != null)
                Controller.ExecuteMessage("printContent", m_events);
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
