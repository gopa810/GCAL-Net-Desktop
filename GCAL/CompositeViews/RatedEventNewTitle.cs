using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GCAL.Base;
using GCAL.Base.Properties;
using GCAL.Views;

namespace GCAL.CompositeViews
{
    public partial class RatedEventNewTitle : UserControl
    {
        public RatedEventNewTitleController Controller { get; set; }

        public GCConfigRatedEvents ExistingConfiguration { get; set; }

        public event TBButtonPressed OnButtonYes;

        public RatedEventNewTitle()
        {
            InitializeComponent();
            ExistingConfiguration = null;
        }

        public string ButtonOkLabel
        {
            set
            {
                buttonOK.Text = value;
            }
            get
            {
                return buttonOK.Text;
            }
        }

        public string TextLabel
        {
            set
            {
                label1.Text = value;
            }
        }

        public string TextValue
        {
            set
            {
                textBox1.Text = value;
            }
            get
            {
                return textBox1.Text;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (OnButtonYes != null) OnButtonYes(this, e);
            Controller.RemoveFromContainer();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Controller.RemoveFromContainer();
        }

    }

    public class RatedEventNewTitleController : GVCore
    {
        public RatedEventNewTitleController(RatedEventNewTitle v)
        {
            View = v;
            v.Controller = this;
        }
    }
}
