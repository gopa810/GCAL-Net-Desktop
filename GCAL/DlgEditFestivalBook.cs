using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GCAL
{
    public partial class DlgEditFestivalBook : Form
    {
        public DlgEditFestivalBook()
        {
            InitializeComponent();
        }

        public string CollectionTitle
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }
    }
}
