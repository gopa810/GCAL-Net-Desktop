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
    public partial class TimezoneDetails : UserControl
    {
        public TTimeZone SelectedTimeZone { get; set; }
        public TimezoneDetailsController Controller { get; set; }

        public event TBButtonPressed OnButtonSave;
        public event TBButtonPressed OnButtonCancel;

        public TimezoneDetails()
        {
            InitializeComponent();
            SelectedTimeZone = null;
        }

        private void panel1_SizeChanged(object sender, EventArgs e)
        {
            if (panel2.Size.Width > dstStartPanel.Size.Width + dstEndPanel.Size.Width + 8)
            {
                label7.Location = new Point(label6.Bounds.Right + 8, label6.Location.Y);
                dstEndPanel.Location = new Point(label7.Location.X, label7.Bounds.Bottom + 8);
            }
            else
            {
                label7.Location = new Point(label6.Location.X, dstStartPanel.Bounds.Bottom + 8);
                dstEndPanel.Location = new Point(label7.Location.X, label7.Bounds.Bottom + 8);
            }

            panel2.Size = new Size(panel2.Size.Width, dstEndPanel.Bounds.Bottom + 8);
        }

        public void setTimeZone(TTimeZone tz)
        {
            if (tz == null)
            {
                textBox1.Text = "Untitled/Timezone";
                offsetNoDST.ValueMinutes = 0;
                checkBox1.Checked = false;
            }
            else
            {
                textBox1.Text = tz.Name;
                offsetNoDST.ValueMinutes = tz.OffsetMinutes;
                if (tz.BiasMinutes == 0)
                {
                    checkBox1.Checked = false;
                    offsetDST.ValueMinutes = 0;
                    dstStartPanel.Value = new TTimeZoneDst();
                    dstEndPanel.Value = new TTimeZoneDst();
                }
                else
                {
                    checkBox1.Checked = true;
                    offsetDST.ValueMinutes = tz.OffsetMinutes + tz.BiasMinutes;
                    dstStartPanel.Value = tz.StartDst;
                    dstEndPanel.Value = tz.EndDst;
                }
            }

            SelectedTimeZone = tz;
        }

        public void setCopyTimeZone(TTimeZone tz)
        {
            setTimeZone(tz);
            textBox1.Text = textBox1.Text + " (copy)";
            SelectedTimeZone = null;
        }

        public void SaveTimeZoneData()
        {
            bool changed = false;
            bool created = false;

            if (SelectedTimeZone == null)
            {
                SelectedTimeZone = new TTimeZone();
                created = true;
            }

            if (SelectedTimeZone != null)
            {
                if (SelectedTimeZone.Name == null || !SelectedTimeZone.Name.Equals(textBox1.Text))
                {
                    SelectedTimeZone.Name = textBox1.Text;
                    changed = true;
                }
                if (SelectedTimeZone.OffsetMinutes != offsetNoDST.ValueMinutes)
                {
                    SelectedTimeZone.OffsetMinutes = offsetNoDST.ValueMinutes;
                    changed = true;
                }
                if (SelectedTimeZone.BiasMinutes != offsetDST.ValueMinutes - offsetNoDST.ValueMinutes)
                {
                    SelectedTimeZone.BiasMinutes = offsetDST.ValueMinutes - offsetNoDST.ValueMinutes;
                    changed = true;
                }
                if (checkBox1.Checked && SelectedTimeZone.BiasMinutes != 0)
                {
                    if (SelectedTimeZone.StartDst.IntegerValue != dstStartPanel.Value.IntegerValue
                        || SelectedTimeZone.EndDst.IntegerValue != dstEndPanel.Value.IntegerValue)
                    {
                        SelectedTimeZone.StartDst = dstStartPanel.Value;
                        SelectedTimeZone.EndDst = dstEndPanel.Value;
                        changed = true;
                    }
                }
                else
                {
                    if (SelectedTimeZone.StartDst.IntegerValue != 0)
                    {
                        SelectedTimeZone.StartDst.Clear();
                        changed = true;
                    }
                    if (SelectedTimeZone.EndDst.IntegerValue != 0)
                    {
                        SelectedTimeZone.EndDst.Clear();
                        changed = true;
                    }
                }

                if (created)
                {
                    TTimeZone.TimeZoneList.Add(SelectedTimeZone);
                    changed = true;
                }

                if (changed)
                {
                    TTimeZone.Modified = true;
                }
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            panel2.Visible = checkBox1.Checked;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveTimeZoneData();
            Controller.RemoveFromContainer();

            if (OnButtonSave != null)
                OnButtonSave(this, e);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Controller.RemoveFromContainer();
            if (OnButtonCancel != null)
                OnButtonCancel(this, e);
        }


    }

    /// <summary>
    /// Controller for Timezone Details view
    /// </summary>
    public class TimezoneDetailsController : GVCore
    {
        public TimezoneDetailsController(TimezoneDetails v)
        {
            View = v;
            v.Controller = this;
        }
    }
}
