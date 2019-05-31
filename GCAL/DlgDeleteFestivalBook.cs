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
    public partial class DlgDeleteFestivalBook : Form
    {
        public DlgDeleteFestivalBook()
        {
            InitializeComponent();
        }

        public string FileNameReimport
        {
            set
            {
                label7.Text = value;
            }
        }
    }
}
