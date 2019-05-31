using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GCAL.Base;
using GCAL.Base.Scripting;
using GCAL.Views;

namespace GCAL.CompositeViews
{
    public partial class EditEventBase : UserControl
    {
        public event TBButtonPressed OnButtonOK;
        public event TBButtonPressed OnButtonCancel;

        public EditEventBase()
        {
            InitializeComponent();

            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();
            foreach (GCFestivalBook book in GCFestivalBookCollection.Books)
            {
                comboBox1.Items.Add(book.CollectionName);
            }
            comboBox1.EndUpdate();

            comboBox4.BeginUpdate();
            foreach (FastType ft in FastType.Fasts)
            {
                comboBox4.Items.Add(ft);
            }
            comboBox4.EndUpdate();
            comboBox4.SelectedIndex = 0;

            SetButtonsVisible(false);
        }

        public void SetButtonsVisible(bool bVisible)
        {
            buttonCancel.Visible = bVisible;
            buttonOK.Visible = bVisible;
        }

        public string EventTitle
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value;  }
        }

        public string EventFastingSubject
        {
            get { return textBox2.Text; }
            set { textBox2.Text = value; }
        }

        public int EventStartYear
        {
            get { return Convert.ToInt32(numericUpDown1.Value); }
            set { numericUpDown1.Value = value; }
        }

        public int Fast
        {
            get
            {
                if (comboBox4.SelectedIndex >= 0 && comboBox4.SelectedIndex < comboBox4.Items.Count)
                {
                    object o = comboBox4.Items[comboBox4.SelectedIndex];
                    if (o is FastType)
                    {
                        return ((FastType)o).FastID;
                    }
                }
                return 0;
            }
            set
            {
                int i = 0;
                foreach (FastType ft in FastType.Fasts)
                {
                    if (ft.FastID == value)
                    {
                        comboBox4.SelectedIndex = i;
                        return;
                    }
                    i++;
                }
            }
        }

        public int EventBook
        {
            get
            {
                int cs = comboBox1.SelectedIndex;
                if (cs >= 0 && cs < comboBox1.Items.Count)
                    return cs;
                return 0;
            }
            set
            {
                if (value >= -1 && value < comboBox1.Items.Count)
                    comboBox1.SelectedIndex = value;
            }
        }

        public int DaysOffset
        {
            get { return Convert.ToInt32(numericUpDown2.Value); }
            set { numericUpDown2.Value = value; }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (OnButtonOK != null)
                OnButtonOK(this, e);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (OnButtonCancel != null)
                OnButtonCancel(this, e);
        }

    }
}
