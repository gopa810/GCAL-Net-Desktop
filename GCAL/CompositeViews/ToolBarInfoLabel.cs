using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GCAL.Base;

namespace GCAL.CompositeViews
{
    public delegate void TBButtonPressed(object sender, EventArgs e);

    public partial class ToolBarInfoLabel : UserControl
    {
        public event TBButtonPressed ButtonPressed;

        public ToolBarInfoLabel()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ButtonPressed != null)
                ButtonPressed(this, e);
        }

        public string ButtonTitle
        {
            get
            {
                return button1.Text;
            }
            set
            {
                button1.Text = value;
            }
        }

        public string LabelText
        {
            get
            {
                return label2.Text;
            }
            set
            {
                label2.Text = value;
            }
        }
    }
}
