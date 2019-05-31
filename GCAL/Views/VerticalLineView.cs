using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GCAL.Views
{
    public partial class VerticalLineView : UserControl
    {
        public VerticalLineView()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawLine(Pens.Black, this.Width / 2, 0, this.Width / 2, this.Height);
            base.OnPaint(e);
        }

    }
}
