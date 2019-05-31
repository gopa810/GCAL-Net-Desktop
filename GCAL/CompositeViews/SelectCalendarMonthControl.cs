using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GCAL.CompositeViews
{
    public partial class SelectCalendarMonthControl : UserControl
    {
        int lastValidYear = DateTime.Now.Year;

        public event OnMonthSelectedDelegate MonthSelected;

        public SelectCalendarMonthControl()
        {
            InitializeComponent();
            textBox1.Text = lastValidYear.ToString();
        }

        private void buttonSelectMonth_Click(object sender, EventArgs e)
        {
            SelectMonthEventArgs args = new SelectMonthEventArgs();

            args.Month = int.Parse((sender as Button).Tag.ToString());
            args.Year = GetSelectedYear();

            if (MonthSelected != null)
                MonthSelected(sender, args);
        }

        private void buttonGoPrev_Click(object sender, EventArgs e)
        {
            textBox1.Text = LimitYear(GetSelectedYear() - 1).ToString();
        }

        private void buttonGoNext_Click(object sender, EventArgs e)
        {
            textBox1.Text = LimitYear(GetSelectedYear() + 1).ToString();
        }

        public int GetSelectedYear()
        {
            int y;
            if (int.TryParse(textBox1.Text, out y))
            {
                return LimitYear(y);
            }

            return DateTime.Now.Year;
        }

        private int LimitYear(int y)
        {
            if (y < 1600)
                y = 1600;
            if (y > 2999)
                y = 2999;
            return y;
        }
    }

    public class SelectMonthEventArgs: EventArgs
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public delegate void OnMonthSelectedDelegate(object o, SelectMonthEventArgs arg);
}
