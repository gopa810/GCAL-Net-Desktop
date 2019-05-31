using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GCAL.Base;

namespace GCAL.CompositeViews
{
    public partial class DateSpecifyDstPanel : UserControl
    {
        public TTimeZoneDst Value
        {
            get
            {
                TTimeZoneDst val = new TTimeZoneDst();

                val.Month = comboBoxMonth.SelectedIndex + 1;
                val.Type = comboBoxType.SelectedIndex;
                if (val.Type == 0)
                {
                    val.Day = comboBoxDayOfWeek.SelectedIndex;
                    val.Week = comboBoxWeekOfMonth.SelectedIndex + 1;
                }
                else
                {
                    val.Day = comboBoxDayOfMonth.SelectedIndex + 1;
                }

                return val;
            }
            set
            {
                if (value.Month < 12)
                    comboBoxMonth.SelectedIndex = value.Month - 1;
                if (value.Type < 2)
                    comboBoxType.SelectedIndex = value.Type;
                if (value.Type == 0)
                {
                    comboBoxDayOfWeek.SelectedIndex = value.Day;
                    comboBoxWeekOfMonth.SelectedIndex = value.Week - 1;
                    comboBoxDayOfMonth.SelectedIndex = -1;
                }
                else if (value.Type == 1)
                {
                    comboBoxDayOfWeek.SelectedIndex = -1;
                    comboBoxWeekOfMonth.SelectedIndex = -1;
                    comboBoxDayOfMonth.SelectedIndex = value.Day - 1;
                }
            }
        }

        public DateSpecifyDstPanel()
        {
            InitializeComponent();
            int i;

            for (i = 0; i < 7; i++)
            {
                comboBoxDayOfWeek.Items.Add(GCCalendar.GetWeekdayName(i));
            }

            for (i = 1; i < 32; i++)
            {
                comboBoxDayOfMonth.Items.Add(i.ToString());
            }

            for (i = 1; i < 13; i++)
            {
                comboBoxMonth.Items.Add(GregorianDateTime.GetMonthName(i));
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool en1 = (comboBoxType.SelectedIndex == 0);

            label2.Enabled = en1;
            label3.Enabled = en1;
            comboBoxDayOfWeek.Enabled = en1;
            comboBoxWeekOfMonth.Enabled = en1;

            label5.Enabled = !en1;
            comboBoxDayOfMonth.Enabled = !en1;
        }
    }
}
