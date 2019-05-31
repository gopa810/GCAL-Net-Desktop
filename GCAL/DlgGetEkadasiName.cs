using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GCAL.Base;

namespace GCAL
{
    public partial class DlgGetEkadasiName : Form
    {
        public VAISNAVADAY SelectedDate = null;

        public DlgGetEkadasiName()
        {
            DateTime dt = DateTime.Now;
            InitializeComponent();
            comboBox1.SelectedIndex = dt.Month - 1;
            panel1.Visible = false;
            textBox1.Text = dt.Year.ToString();
        }

        private void buttonSelectEkadasi_Click(object sender, EventArgs e)
        {
            SelectedDate = (listBox1.Items[listBox1.SelectedIndex] as LBVD).vd;
        }

        private void buttonCalcEkadasi_Click(object sender, EventArgs e)
        {
            TResultCalendar cal = new TResultCalendar();

            cal.CalculateCalendar(GCGlobal.myLocation, new GregorianDateTime(int.Parse(textBox1.Text), comboBox1.SelectedIndex + 1, 1), 31);
            List<VAISNAVADAY> le = new List<VAISNAVADAY>();

            for (int i = 0; i < 31; i++)
            {
                VAISNAVADAY vd = cal.GetDay(i);
                if (vd.nFastID == FastType.FAST_EKADASI)
                {
                    LBVD lb = new LBVD();
                    lb.vd = vd;
                    listBox1.Items.Add(lb);
                }
            }

            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;

            panel1.Visible = true;

        }

        public class LBVD
        {
            public VAISNAVADAY vd = null;

            public override string ToString()
            {
                return vd.date.ToString();
            }
        }
    }
}
