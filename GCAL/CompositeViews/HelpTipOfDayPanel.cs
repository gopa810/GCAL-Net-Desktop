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
    public partial class HelpTipOfDayPanel : UserControl
    {
        public TipOfDayPanelController Controller { get; set; }

        public int NextIndex { get; set; }

        public string[] TipLines { get; set; }

        public HelpTipOfDayPanel()
        {
            InitializeComponent();
            string tips = Properties.Resources.tips;
            TipLines = tips.Split('\n');
            NextIndex = Properties.Settings.Default.TipOfDayIndex;
            checkBox1.Checked = Properties.Settings.Default.ShowStartupTips;
            ShowTipOfDay();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TipOfDayIndex = NextIndex;
            Properties.Settings.Default.ShowStartupTips = checkBox1.Checked;
            Properties.Settings.Default.Save();
            Controller.RemoveFromContainer();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            ShowTipOfDay();
        }

        private void ShowTipOfDay()
        {
            if (TipLines.Length > 0)
            {
                NextIndex = NextIndex % TipLines.Length;
                richTextBox1.Text = TipLines[NextIndex];
                NextIndex = (NextIndex + 1) % TipLines.Length;
            }
        }
    }

    public class TipOfDayPanelController : GVCore
    {
        public TipOfDayPanelController(HelpTipOfDayPanel v)
        {
            View = v;
            v.Controller = this;
        }
    }
}
