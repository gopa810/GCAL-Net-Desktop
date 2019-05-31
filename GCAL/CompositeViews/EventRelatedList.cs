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
    public partial class EventRelatedList : UserControl
    {
        public IControlContainer ViewContainer { get; set; }
        public EventRelatedListController Controller { get; set; }
        public GCFestivalBase EventObject { get; set; }

        private int listBoxItemPadding = 4;
        private Font listBoxTitleFont;
        private Font listBoxSubtitleFont;

        public EventRelatedList()
        {
            InitializeComponent();

            listBoxTitleFont = new Font(FontFamily.GenericSansSerif, 13);
            listBoxSubtitleFont = new Font(FontFamily.GenericSansSerif, 10);

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
                    e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds);
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

        /// <summary>
        /// Fill main listbox with related events
        /// </summary>
        public void FillRelativeEvents()
        {
            listBox1.Items.Clear();
            if (EventObject.EventsCount > 0)
            {
                foreach (GCFestivalRelated r in EventObject.Events)
                {
                    GCListBoxEntry lb = new GCListBoxEntry();
                    r.updateListBoxEntry(lb);
                    listBox1.Items.Add(lb);
                }
            }
        }

        /// <summary>
        /// User interface handler for BACK button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBack_Click(object sender, EventArgs e)
        {
            Controller.RemoveFromContainer();
        }

        /// <summary>
        /// User interface handler for NEW related event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNew_Click(object sender, EventArgs e)
        {
            EventRelatedDetails d = new EventRelatedDetails();
            EventRelatedDetailsController dc = new EventRelatedDetailsController(d);

            dc.ViewContainer = Controller.ViewContainer;
            d.setEvent(null);
            d.setParentEvent(EventObject);
            d.OnEventDetailChanged += new TBButtonPressed(onRelatedEventChanged);

            if (ViewContainer != null)
                ViewContainer.AddControl(dc, GVControlAlign.Fill);
        }

        /// <summary>
        /// User interface handler for EDIT related event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0
                && listBox1.SelectedIndex < listBox1.Items.Count)
            {
                GCListBoxEntry lbe = listBox1.Items[listBox1.SelectedIndex] as GCListBoxEntry;
                if (lbe != null && lbe.Tag != null && lbe.Tag is GCFestivalRelated)
                {
                    GCFestivalRelated r = lbe.Tag as GCFestivalRelated;
                    EventRelatedDetails d = new EventRelatedDetails();
                    EventRelatedDetailsController dc = new EventRelatedDetailsController(d);

                    dc.ViewContainer = Controller.ViewContainer;
                    d.setEvent(r);
                    d.setParentEvent(EventObject);
                    d.OnEventDetailChanged += new TBButtonPressed(onRelatedEventChanged);

                    if (ViewContainer != null)
                        ViewContainer.AddControl(dc, GVControlAlign.Fill);
                }
            }

        }

        /// <summary>
        /// User interface handler for DELETE related event button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0
                && listBox1.SelectedIndex < listBox1.Items.Count)
            {
                GCListBoxEntry lbe = listBox1.Items[listBox1.SelectedIndex] as GCListBoxEntry;
                if (lbe != null && lbe.Tag != null)
                {
                    AskDeleteObject d = new AskDeleteObject();
                    d.Tag = lbe.Tag;
                    d.InitialLabel = "Are you sure to remove following event?";
                    d.DetailLabel = (lbe.Tag as GCFestivalBase).Text;
                    AskDeleteObjectController dc = new AskDeleteObjectController(d);
                    d.OnButtonYes += new TBButtonPressed(askDeleteEvent1_OnButtonYes);
                    if (ViewContainer != null)
                        ViewContainer.AddControl(dc, GVControlAlign.Fill);
                }
            }
        }

        /// <summary>
        /// Deleting related event and hiding dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void askDeleteEvent1_OnButtonYes(object sender, EventArgs e)
        {
            if (sender is AskDeleteObject)
            {
                AskDeleteObject d = sender as AskDeleteObject;
                if (d != null && d.Tag != null && (d.Tag is GCFestivalBase))
                {
                    EventObject.Remove(d.Tag as GCFestivalBase);
                }
            }
            FillRelativeEvents();
        }

        /// <summary>
        /// Callback for change of related events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onRelatedEventChanged(object sender, EventArgs e)
        {
            FillRelativeEvents();
        }
    }

    /// <summary>
    /// Controller for view EventRelatedList
    /// </summary>
    public class EventRelatedListController : GVCore
    {
        public EventRelatedListController(EventRelatedList v)
        {
            View = v;
            v.Controller = this;
        }
    }
}
