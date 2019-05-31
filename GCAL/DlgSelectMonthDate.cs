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
    public partial class DlgSelectMonthDate : Form
    {
        public int SelectedMonth = -1;
        public int SelectedYear = -1;

        public DlgSelectMonthDate()
        {
            InitializeComponent();
        }

        private void selectCalendarMonthControl1_MonthSelected(object o, CompositeViews.SelectMonthEventArgs arg)
        {
            SelectedMonth = arg.Month;
            SelectedYear = arg.Year;
            DialogResult = DialogResult.OK;
        }
    }
}
