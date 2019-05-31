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
    public partial class RatedEventDetailsPanel : UserControl
    {
        public RatedEventDetailsPanelController Controller { get; set; }

        private GCConfigRatedEntry SelectedRatedEntry;

        private string originalEncodedString = null;

        public event TBButtonPressed OnValueSaved;

        public RatedEventDetailsPanel()
        {
            InitializeComponent();
        }

        public GCConfigRatedEntry RatedEvent
        {
            set
            {
                SelectedRatedEntry = value;
                setRatedEvent(value);
            }
            get
            {
                return SelectedRatedEntry;
            }
        }

        private void setRatedEvent(GCConfigRatedEntry ge)
        {
            if (ge != null)
            {
                originalEncodedString = ge.EncodedString;
                label4.Text = ge.Title;
                numericUpDown3.Value = Convert.ToDecimal(ge.Rating);
                if (ge.Note != null)
                    richTextBox2.Text = ge.Note;
                else
                    richTextBox2.Text = "";

                InitListbox(ge);
            }
        }

        private void InitListbox(GCConfigRatedEntry ge)
        {
            listBox1.BeginUpdate();
            listBox1.Items.Clear();
            for (int i = 0; i < ge.MarginsCount; i++)
            {
                ListBoxRichItem lbi = new ListBoxRichItem();
                lbi.Tag = ge[i];
                lbi.Title = ge[i].Title;
                lbi.Subtitle = string.Format("Period {0} min - {1} min rel to starting time",
                    ge[i].OffsetMinutesStart, ge[i].OffsetMinutesEnd);
                listBox1.Items.Add(lbi);
            }
            listBox1.EndUpdate();
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (SelectedRatedEntry != null)
            {
                SelectedRatedEntry.Note = richTextBox2.Text;
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (SelectedRatedEntry != null)
            {
                SelectedRatedEntry.Rating = Convert.ToDouble(numericUpDown3.Value);
            }
        }

        private void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index >= 0 && e.Index < listBox1.Items.Count)
            {
                e.ItemHeight = (listBox1.Items[e.Index] as ListBoxRichItem).MeasureItem(e.Graphics, listBox1.Bounds, 1);
            }
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0 && e.Index < listBox1.Items.Count)
            {
                ListBoxRichItem lbi = listBox1.Items[e.Index] as ListBoxRichItem;
                lbi.DrawItem(e, 1);

                if (lbi.Tag != null && lbi.Tag is GCConfigRatedEntry)
                {
                    GCConfigRatedEntry ge = lbi.Tag as GCConfigRatedEntry;
                    string s = ge.Rating.ToString();
                    SizeF ss = e.Graphics.MeasureString(s, ListBoxRichItem.H1Font);
                    Brush b = Brushes.Gray;
                    if (ge.Rating > 0.0) b = Brushes.DarkGreen;
                    else if (ge.Rating < 0.0) b = Brushes.DarkRed;
                    e.Graphics.DrawString(s, ListBoxRichItem.H1Font, b, e.Bounds.Right - 8 - ss.Width, e.Bounds.Top + 8);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (SelectedRatedEntry != null)
            {
                if (!SelectedRatedEntry.EncodedString.Equals(originalEncodedString))
                {
                    SelectedRatedEntry.Modified = true;
                }
            }
            if (OnValueSaved != null)
            {
                OnValueSaved(this, e);
            }
            Controller.RemoveFromContainer();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (SelectedRatedEntry != null)
                SelectedRatedEntry.EncodedString = originalEncodedString;
            Controller.RemoveFromContainer();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            RatedSubeventPanel d = new RatedSubeventPanel();
            d.setSubevent(null);
            d.OnSubeventDone += new TBButtonPressed(d_OnSubeventDone);
            RatedSubeventPanelController dc = new RatedSubeventPanelController(d);
            dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Center);
        }

        private void d_OnSubeventDone(object sender, EventArgs e)
        {
            if (sender is RatedSubeventPanel)
            {
                RatedSubeventPanel d = sender as RatedSubeventPanel;
                if (d.InputSubevent == null)
                {
                    RatedEvent.AddMargin(d.OutputSubevent);
                }
                InitListbox(RatedEvent);
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            RatedSubeventPanel d = new RatedSubeventPanel();
            d.setSubevent(SelectedSubevent);
            d.OnSubeventDone += new TBButtonPressed(d_OnSubeventDone);
            RatedSubeventPanelController dc = new RatedSubeventPanelController(d);
            dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Center);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            GCConfigRatedMargin sel = SelectedSubevent;
            if (sel != null)
            {
                AskDeleteObject d = new AskDeleteObject();
                d.InitialLabel = "Delete rated subevent?";
                d.DetailLabel = sel.Title;
                d.Tag = sel;
                d.OnButtonYes += new TBButtonPressed(d_OnButtonYes);
                AskDeleteObjectController dc = new AskDeleteObjectController(d);
                dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Center);
            }
        }

        private void d_OnButtonYes(object sender, EventArgs e)
        {
            if (sender is AskDeleteObject)
            {
                AskDeleteObject d = sender as AskDeleteObject;
                if (d.Tag is GCConfigRatedMargin)
                {
                    RatedEvent.DeleteMargin(d.Tag as GCConfigRatedMargin);
                    InitListbox(RatedEvent);
                }
            }
        }

        private GCConfigRatedMargin SelectedSubevent
        {
            get
            {
                if (listBox1.SelectedIndex >= 0 && listBox1.SelectedIndex < listBox1.Items.Count)
                {
                    object item = listBox1.Items[listBox1.SelectedIndex];
                    if (item is ListBoxRichItem)
                    {
                        object tag = (item as ListBoxRichItem).Tag;
                        if (tag is GCConfigRatedMargin)
                            return tag as GCConfigRatedMargin;
                    }
                }

                return null;
            }
        }
        private int SelectedIndex
        {
            get
            {
                if (listBox1.SelectedIndex >= 0 && listBox1.SelectedIndex < listBox1.Items.Count)
                {
                    return listBox1.SelectedIndex;
                }

                return -1;
            }
        }
    }

    public class RatedEventDetailsPanelController : GVCore
    {
        public RatedEventDetailsPanelController(RatedEventDetailsPanel v)
        {
            View = v;
            v.Controller = this;
        }
    }
}
