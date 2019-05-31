using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.Remoting.Messaging;

using GCAL.Base;
using GCAL.Base.Scripting;
using GCAL.Views;
using GCAL.CompositeViews;

namespace GCAL.CompositeViews
{
    public partial class RatedEventsTab : UserControl
    {
        TResultRatedEvents m_events;
        public RatedEventsTabController Controller { get; set; }
        private int p_mode = 0;
        public GCConfigRatedEvents SelectedConfiguration = null;
        private string textToDisplayTxt = "";
        private string textToDisplayRtf = "";
        private bool doNotRecalculate = false;

        public GCLocation ratedLocation = null;
        public GregorianDateTime ratedStartDate = new GregorianDateTime();
        public GregorianDateTime ratedEndDate = new GregorianDateTime();


        public RatedEventsTab()
        {
            InitializeComponent();
            richTextBox1.Dock = DockStyle.Fill;
            pictureBox1.Dock = DockStyle.Fill;

            string s = Properties.Settings.Default.RatedLocation;
            if (s.Length < 1)
                s = GCGlobal.LastLocation.EncodedString;
            ratedLocation = new GCLocation();
            ratedLocation.EncodedString = s;
            s = Properties.Settings.Default.RatedStartDate;
            if (s.Length > 1)
                ratedStartDate.EncodedString = s;
            s = Properties.Settings.Default.RatedEndDate;
            if (s.Length > 1)
                ratedEndDate.EncodedString = s;

            InitTemplates();
            s = Properties.Settings.Default.RatedConfiguration;
            SelectedConfiguration = GCConfigRatedManager.GetConfiguration(s);

            bEnchit = true;
            templateComboBox.SelectedIndex = templateComboBox.Items.IndexOf(SelectedConfiguration.Title);
            bEnchit = false;

            SetMode(Properties.Settings.Default.RatedEventsShowMode);

        }

        public void Start()
        {
            //Recalculate();
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
                //richTextBox1.Visible = true;
                //pictureBox1.Visible = false;

                p_mode = i;
                DisplayCalendarResult();
                if (Properties.Settings.Default.RatedEventsShowMode != i)
                {
                    Properties.Settings.Default.RatedEventsShowMode = i;
                    Properties.Settings.Default.Save();
                }
            }
            else if (i == 1)
            {
                //richTextBox1.Visible = true;
                //pictureBox1.Visible = false;

                p_mode = i;
                DisplayCalendarResult();
                if (Properties.Settings.Default.RatedEventsShowMode != i)
                {
                    Properties.Settings.Default.RatedEventsShowMode = i;
                    Properties.Settings.Default.Save();
                }
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
            string s = ratedLocation.EncodedString;
            if (!s.Equals(Properties.Settings.Default.RatedLocation))
            {
                Properties.Settings.Default.RatedLocation = s;
                settingsChanged = true;
            }
            s = ratedStartDate.EncodedString;
            if (!s.Equals(Properties.Settings.Default.RatedStartDate))
            {
                Properties.Settings.Default.RatedStartDate = s;
                settingsChanged = true;
            }
            s = ratedEndDate.EncodedString;
            if (!s.Equals(Properties.Settings.Default.RatedEndDate))
            {
                Properties.Settings.Default.RatedEndDate = s;
                settingsChanged = true;
            }
            if (settingsChanged)
            {
                Properties.Settings.Default.Save();
            }

            if (p_mode == 0 || p_mode == 1)
            {
                LocationText(ratedLocation.Title);
                StartDateText(ratedStartDate.ToString());
                EndDateText(ratedEndDate.ToString());

                richTextBox1.Visible = false;
                pictureBox1.Visible = true;

                doNotRecalculate = false;
                backgroundWorker1.RunWorkerAsync();
/*                m_events = new TResultRatedEvents();
                m_events.CompleteCalculation(ratedLocation, ratedStartDate, ratedEndDate, SelectedConfiguration);
                CalcDeleg caller = new CalcDeleg(m_events.CompleteCalculation);
                IAsyncResult result = caller.BeginInvoke(ratedLocation, ratedStartDate, ratedEndDate, SelectedConfiguration,
                    new AsyncCallback(RecalculationDone),
                    "Recalculation");*/

                //DisplayCalendarResult();
            }
        }

        private delegate void MainThrDeleg();
        public delegate void CalcDeleg(GCLocation loc, GregorianDateTime sd, GregorianDateTime ed, GCConfigRatedEvents sc);

        public void RecalculationDone(IAsyncResult ar)
        {
            MainThrDeleg mt = new MainThrDeleg(RecalculationDoneMainThread);
            this.Invoke(mt);
        }

        public void RecalculationDoneMainThread()
        {
            if (textToDisplayTxt.Length > 0) richTextBox1.Text = textToDisplayTxt;
            else if (textToDisplayRtf.Length > 0) richTextBox1.Rtf = textToDisplayRtf;
            richTextBox1.Visible = true;
            pictureBox1.Visible = false;
        }

        public TResultBase getCurrentContent()
        {
            return m_events;
        }

        /// <summary>
        /// Reinitiate the list in combobox
        /// and maintain selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnConfigurationListChanged(object sender, EventArgs e)
        {
            InitTemplates();
            int i = templateComboBox.Items.IndexOf(SelectedConfiguration.Title);
            if (i < 0) i = 0;
            bEnchit = true;
            templateComboBox.SelectedIndex = i;
            bEnchit = false;
        }

        public void DisplayCalendarResult()
        {
            doNotRecalculate = true;
            richTextBox1.Visible = false;
            pictureBox1.Visible = true;
            backgroundWorker1.RunWorkerAsync();
/*            if (p_mode == 0)
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
            }*/
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
                ratedLocation = lr;
                Recalculate();
            }
        }

        private void onDateRangeClick(object sender, EventArgs e)
        {
            EnterPeriodPanel d = new EnterPeriodPanel();
            d.EarthLocation = ratedLocation.GetEarthData();
            d.OnPeriodSelected += new TBButtonPressed(d_OnPeriodSelected);
            d.InputStartDate = ratedStartDate;
            d.InputEndDate = ratedEndDate;
            EnterPeriodPanelController dc = new EnterPeriodPanelController(d);
            dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Center);
        }

        private void d_OnPeriodSelected(object sender, EventArgs e)
        {
            if (sender is EnterPeriodPanel)
            {
                EnterPeriodPanel d = sender as EnterPeriodPanel;
                ratedStartDate = d.InputStartDate;
                ratedEndDate = d.InputEndDate;
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

        public bool bEnchit = false;

        public void InitTemplates()
        {
            templateComboBox.Items.Clear();
            foreach (GCConfigRatedEvents re in GCConfigRatedManager.Configurations)
            {
                templateComboBox.Items.Add(re.Title);
            }
            templateComboBox.Items.Add("<open editor>");
        }

        private int p_lastSelectedTemplateIndex = -1;

        private void templateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bEnchit)
                return;

            if (templateComboBox.SelectedIndex >= 0 && templateComboBox.SelectedIndex < templateComboBox.Items.Count)
            {
                if (templateComboBox.SelectedIndex == templateComboBox.Items.Count - 1)
                {
                    bEnchit = true;
                    templateComboBox.SelectedIndex = p_lastSelectedTemplateIndex;
                    bEnchit = false;

                    RatedEventsEditor d = new RatedEventsEditor();
                    d.OnConfListChanged += new TBButtonPressed(OnConfigurationListChanged);
                    RatedEventsEditorController dc = new RatedEventsEditorController(d);
                    dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Fill);
                }
                else
                {
                    SelectedConfiguration = GCConfigRatedManager.Configurations[templateComboBox.SelectedIndex];
                    Properties.Settings.Default.RatedConfiguration = SelectedConfiguration.Title;
                    Properties.Settings.Default.Save();
                    Recalculate();
                    p_lastSelectedTemplateIndex = templateComboBox.SelectedIndex;
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {/*
            MainThrDeleg mt = new MainThrDeleg(RecalculationDoneMainThread);
            this.Invoke(mt);*/

            if (textToDisplayTxt.Length > 0) richTextBox1.Text = textToDisplayTxt;
            else if (textToDisplayRtf.Length > 0) richTextBox1.Rtf = textToDisplayRtf;
            richTextBox1.Visible = true;
            pictureBox1.Visible = false;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!doNotRecalculate || m_events == null)
            {
                m_events = new TResultRatedEvents();
                m_events.CompleteCalculation(ratedLocation, ratedStartDate, ratedEndDate, SelectedConfiguration);
            }

            doNotRecalculate = false;

            if (p_mode == 0)
            {
                textToDisplayTxt = m_events.formatText(GCDataFormat.PlainText);
                textToDisplayRtf = "";
            }
            else if (p_mode == 1)
            {
                textToDisplayTxt = "";
                textToDisplayRtf = m_events.formatText(GCDataFormat.Rtf);
            }
        }

    }
}
