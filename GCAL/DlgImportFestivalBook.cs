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
    public partial class DlgImportFestivalBook : Form
    {
        public int ResultWay = 0;

        public DlgImportFestivalBook()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResultWay = 1;
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
