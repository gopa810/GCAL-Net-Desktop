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
    public partial class HorizontalLineView : UserControl
    {
        public HorizontalLineView()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawLine(Pens.Black, 0, this.Height / 2, this.Width, this.Height / 2);
            base.OnPaint(e);
        }
    }
}
