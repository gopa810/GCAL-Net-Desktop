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
    public partial class HourMinutesPanel : UserControl
    {
        private bool _nof = true;

        public HourMinutesPanel()
        {
            InitializeComponent();

            InitHours(_nof);
        }

        private void InitHours(bool b)
        {
            int A = b ? -14 : 0;
            int B = b ? 14 : 24;
            comboBox1.Items.Clear();
            for (int i = A; i <= B; i++)
            {
                comboBox1.Items.Add(i.ToString());
            }

        }

        public bool NegativeOffsets
        {
            get
            {
                return _nof;
            }
            set
            {
                _nof = value;
                InitHours(value);
            }
        }

        public string Title
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
            }
        }

        public int ValueMinutes
        {
            get
            {
                if (_nof)
                {
                    int hr = (comboBox1.SelectedIndex - 14) * 60;
                    int min = comboBox2.SelectedIndex * 15;
                    if (hr < 0)
                        return hr - min;
                    else
                        return hr + min;
                }
                else
                {
                    int hr = comboBox1.SelectedIndex * 60;
                    int min = comboBox2.SelectedIndex * 15;
                    if (hr < 0)
                        return hr - min;
                    else
                        return hr + min;
                }
            }
            set
            {
                int H, M, S = (_nof ? 14 : 0);
                int min = Convert.ToInt32(value);
                if (min < 0)
                {
                    H = min / 60 + S;
                    M = (-min % 60) / 15;
                }
                else
                {
                    H = min / 60 + S;
                    M = (min % 60) / 15;
                }

                if (H < 0)
                    H = 0;
                if (H > comboBox1.Items.Count - 1)
                    H = comboBox1.Items.Count - 1;

                comboBox1.SelectedIndex = H;
                comboBox2.SelectedIndex = M;

            }
        }
    }
}
