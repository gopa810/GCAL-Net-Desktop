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
    public partial class AskEventType : UserControl
    {
        public event TBButtonPressed OnButtonOK;
        public event TBButtonPressed OnButtonCancel;

        public AskEventTypeController Controller { get; set; }

        private int listBoxItemPadding = 4;
        private Font listBoxTitleFont;
        private Font listBoxSubtitleFont;

        public AskEventType()
        {
            InitializeComponent();

            listBoxTitleFont = new Font(FontFamily.GenericSansSerif, 13);
            listBoxSubtitleFont = new Font(FontFamily.GenericSansSerif, 10);

            listBox1.Items.Add(new GCListBoxEntry(){ Title = "Tithi-Masa based event", 
                Subtitle = "Event determined by tithi and masa and day offset.", 
                Tag = 1});
            listBox1.Items.Add(new GCListBoxEntry(){ Title = "Sankranti based event", 
                Subtitle = "Event determined by sankranti and day offset.", 
                Tag = 2});
            listBox1.Items.Add(new GCListBoxEntry()
            {
                Title = "Ekadasi based event",
                Subtitle = "Event determined by ekadasi in given paksa and masa.",
                Tag = 3
            });
            listBox1.Items.Add(new GCListBoxEntry(){ Title = "Day-Masa event", 
                Subtitle = "Event determined by tithi and masa. Difference with tithi-masa based event is, that in case of day-masa event, if given tithi is last day of masa and tithi is ksaya, then reminder for this event appears on preceding tithi (masa is mandatory).", 
                Tag = 4});

            listBox1.SelectedIndex = 0;
        }


        public GCFestivalBase SelectedObject
        {
            get
            {
                GCListBoxEntry lb = listBox1.Items[listBox1.SelectedIndex] as GCListBoxEntry;
                if (lb != null)
                {
                    switch ((int)lb.Tag)
                    {
                        case 1:
                            return new GCFestivalTithiMasa();
                        case 2:
                            return new GCFestivalSankranti();
                        case 3:
                            return new GCFestivalEkadasi();
                        case 4:
                            return new GCFestivalMasaDay();
                        default:
                            return new GCFestivalTithiMasa();
                    }
                }

                return null;
            }
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= listBox1.Items.Count)
                return;
            if (listBox1.Items[e.Index] is GCListBoxEntry)
            {
                GCListBoxEntry lb = listBox1.Items[e.Index] as GCListBoxEntry;
                if ((e.State & DrawItemState.Selected) != 0)
                {
                    e.Graphics.FillRectangle(Brushes.LightYellow, e.Bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
                }

                if (lb.TitleHeight > 0)
                {
                    e.Graphics.DrawString(lb.Title, listBoxTitleFont, Brushes.Black, 2 * listBoxItemPadding, e.Bounds.Top + listBoxItemPadding);
                }
                if (lb.SubtitleHeight > 0)
                {
                    e.Graphics.DrawString(lb.Subtitle, listBoxSubtitleFont, Brushes.DarkGray, 2 * listBoxItemPadding,
                        e.Bounds.Top + 2 * listBoxItemPadding + lb.TitleHeight);
                }
            }
        }

        private void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= listBox1.Items.Count)
                return;
            if (listBox1.Items[e.Index] is GCListBoxEntry)
            {
                GCListBoxEntry lb = listBox1.Items[e.Index] as GCListBoxEntry;
                if (lb.SubtitleHeight < 0 || lb.TitleHeight < 0)
                {
                    SizeF sz;
                    if (lb.Title.Length > 0)
                    {
                        sz = e.Graphics.MeasureString(lb.Title, listBoxTitleFont);
                        lb.TitleHeight = Convert.ToInt32(sz.Height);
                    }
                    else
                    {
                        lb.TitleHeight = 0;
                    }
                    if (lb.Subtitle.Length > 0)
                    {
                        sz = e.Graphics.MeasureString(lb.Subtitle, listBoxSubtitleFont);
                        lb.SubtitleHeight = Convert.ToInt32(sz.Height);
                    }
                    else
                    {
                        lb.SubtitleHeight = 0;
                    }
                }
                e.ItemHeight = lb.SubtitleHeight + lb.TitleHeight + 3 * listBoxItemPadding;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (OnButtonOK != null)
                OnButtonOK(this, e);
            Controller.RemoveFromContainer();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (OnButtonCancel != null)
                OnButtonCancel(this, e);
            Controller.RemoveFromContainer();
        }
    }

    public class AskEventTypeController : GVCore
    {
        public AskEventTypeController(AskEventType v)
        {
            View = v;
            v.Controller = this;
        }
    }

}
