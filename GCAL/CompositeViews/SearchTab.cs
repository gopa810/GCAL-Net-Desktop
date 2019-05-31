using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using GCAL.Base;
using GCAL.Base.Scripting;
using GCAL.Views;

namespace GCAL.CompositeViews
{
    public partial class SearchTab : UserControl
    {
        public GCLocation EarthLocation = null;
        public SearchTabController Controller { get; set; }

        public SearchTab()
        {
            InitializeComponent();

            EarthLocation = GCGlobal.myLocation;

            buttonLocation.Text = EarthLocation.GetFullName();

            pictureBox1.Visible = false;

        }

        private CancellationTokenSource ctPrev = null;
        private Task ctTask = null;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Recalculate();
            return;
        }

        public void Recalculate()
        {
            if (ctPrev != null)
            {
                ctPrev.Cancel();
                ctPrev = null;
            } 
            
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            Task t = new Task(() =>
            {
                doSearch(textBox1.Text, tokenSource);
            }, tokenSource.Token);

            t.Start();
            ctTask = t;
            ctPrev = tokenSource;

            pictureBox1.Visible = true;
        }

        /// <summary>
        /// If newText is changed during the processing of this method
        /// then running of this method is aborted and new search starts in backSearch_DoWork
        /// by assigning newText value into currentText
        /// </summary>
        private void doSearch(string text, CancellationTokenSource tokenSource)
        {
            TResultCalendar cal = new TResultCalendar();
            string dayText;
            cal.CalculateCalendar(EarthLocation, new GregorianDateTime(), 700);

            StringBuilder results3 = new StringBuilder();

            GSScript script = new GSScript();
            script.readTextTemplate(TResultCalendar.getPlainDayTemplate());

            GSExecutor exec = new GSExecutor();


            for (int i = 0; i < cal.m_PureCount; i++)
            {
                VAISNAVADAY vd = cal.GetDay(i);

                exec.SetVariable("app", GCUserInterface.Shared);
                exec.SetVariable("location", EarthLocation);
                exec.SetVariable("pvd", vd);

                //dayText = TResultCalendar.formatPlainTextDay(vd);
                exec.resetOutput();
                exec.ExecuteElement(script);
                dayText = exec.getOutput();
                //Debugger.Log(0, "", "SEARCH IN TEXT: " + dayText + "\n\n");
                if (dayText.IndexOf(text, 0, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    results3.AppendLine();
                    results3.AppendLine(dayText);
                }
                if (tokenSource.IsCancellationRequested)
                    return;

            }

            if (!tokenSource.IsCancellationRequested)
            {
                FinalizeDelegate dlg = new FinalizeDelegate(setRichText);
                richTextBox1.BeginInvoke(dlg, results3.ToString());
                ctTask = null;
                ctPrev = null;
            }
        }

        private delegate void FinalizeDelegate(string s);

        private void setRichText(string str)
        {
            richTextBox1.Text = str;
            pictureBox1.Visible = false;
        }

        public void ViewClosing()
        {
            if (ctPrev != null)
            {
                ctPrev.Cancel();
                ctPrev = null;
            }

            int a = 0;
            while ((ctTask.IsCompleted || ctTask.IsCanceled || ctTask.IsFaulted) && a < 100)
            {
                System.Threading.Thread.Sleep(100);
                a++;
            }
        }

        public string LocationText(string s)
        {
            if (s != null)
            {
                buttonLocation.Text = s;
            }
            return buttonLocation.Text;
        }

        private void buttonLocation_Click(object sender, EventArgs e)
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
                LocationText(lr.Title);
                Recalculate();
            }
        }
    }
}
