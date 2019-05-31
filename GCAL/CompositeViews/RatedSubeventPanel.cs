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
    public partial class RatedSubeventPanel : UserControl
    {
        public RatedSubeventPanelController Controller { get; set; }

        public GCConfigRatedMargin InputSubevent { get; set; }

        public GCConfigRatedMargin OutputSubevent { get; set; }

        public event TBButtonPressed OnSubeventDone;

        public void setSubevent(GCConfigRatedMargin margin)
        {
            this.InputSubevent = margin;

            if (margin == null)
            {
            }
            else
            {
                textBox1.Text = InputSubevent.Title;
                richTextBox1.Text = ((InputSubevent.Note != null) ? InputSubevent.Note : "");
                numericUpDown1.Value = Convert.ToDecimal(InputSubevent.Rating);
                numericUpDown2.Value = Convert.ToDecimal(InputSubevent.OffsetMinutesStart);
                numericUpDown3.Value = Convert.ToDecimal(InputSubevent.OffsetMinutesEnd);
            }
        }

        public RatedSubeventPanel()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Controller.RemoveFromContainer();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (InputSubevent == null)
                OutputSubevent = new GCConfigRatedMargin();
            else
                OutputSubevent = InputSubevent;

            OutputSubevent.Title = textBox1.Text;
            OutputSubevent.Note = ((richTextBox1.Text.Length > 0) ? richTextBox1.Text : null);
            OutputSubevent.Rating = Convert.ToDouble(numericUpDown1.Value);
            OutputSubevent.OffsetMinutesStart = Convert.ToInt32(numericUpDown2.Value);
            OutputSubevent.OffsetMinutesEnd = Convert.ToInt32(numericUpDown3.Value);

            if (OnSubeventDone != null)
                OnSubeventDone(this, e);

            Controller.RemoveFromContainer();
        }
    }

    public class RatedSubeventPanelController : GVCore
    {
        public RatedSubeventPanelController(RatedSubeventPanel v)
        {
            View = v;
            v.Controller = this;
        }
    }
}
