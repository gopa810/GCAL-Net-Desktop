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
    public partial class AskDeleteObject : UserControl
    {
        public event TBButtonPressed OnButtonYes;
        public event TBButtonPressed OnButtonNo;

        private object p_tag = null;
        public AskDeleteObjectController Controller { get; set; }

        public AskDeleteObject()
        {
            InitializeComponent();
        }

        public string InitialLabel
        {
            set
            {
                label1.Text = value;
            }
        }

        public string DetailLabel
        {
            set
            {
                label2.Text = value;
            }
        }

        public object Tag
        {
            set
            {
                p_tag = value;
            }
            get
            {
                return p_tag;
            }
        }

        private void AskDeleteEvent_SizeChanged(object sender, EventArgs e)
        {
            int width = this.Size.Width;
            int height = this.Size.Height;

            label1.Location = new Point(label1.Location.X, height/2 - label1.Size.Height - label2.Size.Height - 8);
            label2.Location = new Point(label2.Location.X, height / 2 - label2.Size.Height - 4);
            button1.Location = new Point(width/2 - button1.Size.Width - 2, height/2 + 4);
            button2.Location = new Point(width/2 + 2, height/2 + 4);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (OnButtonYes != null)
                OnButtonYes(this, e);
            Controller.RemoveFromContainer();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (OnButtonNo != null)
                OnButtonNo(this, e);
            Controller.RemoveFromContainer();
        }


    }


    public class AskDeleteObjectController : GVCore
    {
        public AskDeleteObjectController(AskDeleteObject v)
        {
            View = v;
            v.Controller = this;
        }
    }
}
