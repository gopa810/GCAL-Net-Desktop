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
    public partial class EventsPanel : UserControl
    {
        public GCFestivalBook filterToBook = null;
        public bool updatingList = false;
        public bool changingFilter = false;
        private int listBoxItemPadding = 4;
        private Font listBoxTitleFont;
        private Font listBoxSubtitleFont;

        public EventsPanelDelegate Controller { get; set; }

        public EventsPanel()
        {
            InitializeComponent();

            listBoxTitleFont = new Font(FontFamily.GenericSansSerif, 13);
            listBoxSubtitleFont = new Font(FontFamily.GenericSansSerif, 10);

            int i;

            listBoxFestivalBooks.Items.Add("All Collections");
            for (i = 0; i < GCFestivalBookCollection.Books.Count; i++)
            {
                listBoxFestivalBooks.Items.Add(GCFestivalBookCollection.Books[i]);
            }

            listBoxFestivalBooks.SelectedIndex = 0;


            FillListView();
        }

        public void FillListView()
        {
            GCFestivalTithiMasa p;
            updatingList = true;
            listBox1.BeginUpdate();
            listBox1.Items.Clear();

            foreach(GCFestivalBook book in GCFestivalBookCollection.Books)
            {
                if (book.CollectionId >= 0 && book.CollectionId < GCFestivalBookCollection.Books.Count
                    && (filterToBook == null || filterToBook == book))
                {
                    foreach (GCFestivalBase fba in book.Festivals)
                    {
                        if (fba.nDeleted == 0)
                        {
                            GCListBoxEntry lbe = new GCListBoxEntry();
                            fba.updateListBoxEntry(lbe);
                            listBox1.Items.Add(lbe);
                        }
                    }
                }
            }
            updatingList = false;
            listBox1.EndUpdate();
        }

        private void m_wndClass_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private GCListBoxEntry GetSelectedItem()
        {
            if (listBox1.SelectedIndex >= 0 && listBox1.SelectedIndex < listBox1.Items.Count)
            {
                return listBox1.Items[listBox1.SelectedIndex] as GCListBoxEntry;
            }
            return null;
        }

        /// <summary>
        /// OK callback from New Event dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onNewEventOK(object sender, EventArgs e)
        {

            if (sender is AskEventType)
            {
                AskEventType aet = sender as AskEventType;

                GCFestivalBase b = aet.SelectedObject;
                b.Text = "(Untitled)";
                if (b != null)
                {
                    EventDetails d = new EventDetails();
                    EventDetailsController dc = new EventDetailsController(d);
                    dc.ViewContainer = Controller.ViewContainer;
                    d.EventObject = b;
                    d.OnButtonOK += new TBButtonPressed(onNewEventDoneOK);
                    d.OnButtonCancel += new TBButtonPressed(onNewEventDoneCancel);
                    d.OnButtonRelated += new TBButtonPressed(onNewEventDoneRelated);

                    dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Fill);
                }
            }

        }

        private void onNewEventDoneOK(object sender, EventArgs e)
        {
            FillListView();
        }

        private void onNewEventDoneCancel(object sender, EventArgs e)
        {
        }

        private void onNewEventDoneRelated(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Cancel callback from New Event dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onNewEventCancel(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// New Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

            AskEventType d = new AskEventType();
            AskEventTypeController dc = new AskEventTypeController(d);

            if (Controller.ViewContainer != null)
            {
                d.OnButtonOK += new TBButtonPressed(onNewEventOK);
                d.OnButtonCancel += new TBButtonPressed(onNewEventCancel);
                dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Center);
            }
        }

        /// <summary>
        /// Edit event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            GCListBoxEntry item = GetSelectedItem();
            if (item != null && item.Tag != null)
            {
                GCFestivalBase b = item.Tag as GCFestivalBase;
                EventDetails d = new EventDetails();
                d.EventObject = b;
                d.OnButtonOK += new TBButtonPressed(onNewEventDoneOK);
                d.OnButtonCancel += new TBButtonPressed(onNewEventDoneCancel);
                d.OnButtonRelated += new TBButtonPressed(onNewEventDoneRelated);
                EventDetailsController dc = new EventDetailsController(d);
                dc.ShowInContainer(Controller.ViewContainer, GVControlAlign.Fill);
            }
        }

        private void onDeleteEvent(object sender, EventArgs e)
        {
            if (sender is AskDeleteObject)
            {
                AskDeleteObject dlg = sender as AskDeleteObject;
                GCFestivalBase fb = dlg.Tag as GCFestivalBase;
                if (fb != null)
                {
                    GCFestivalBook book = GCFestivalBookCollection.getSafeBook(fb.BookID);
                    book.Remove(fb);
                    onNonDeleteEvent(sender, e);

                    RemoveItemFromListBox(fb);
                }
            }
        }

        private void RemoveItemFromListBox(GCFestivalBase fb)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                GCListBoxEntry lb = listBox1.Items[i] as GCListBoxEntry;
                if (lb.Tag == fb)
                {
                    listBox1.Items.RemoveAt(i);
                    break;
                }
            }
        }

        private void onNonDeleteEvent(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Delete Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            GCListBoxEntry item = GetSelectedItem();

            if (item != null && item.Tag != null && Controller.ViewContainer != null)
            {
                AskDeleteObject dlg = new AskDeleteObject();
                AskDeleteObjectController dlgCtrl = new AskDeleteObjectController(dlg);
                dlg.Tag = item.Tag;
                dlg.InitialLabel = "Are you sure to remove following event?";
                dlg.DetailLabel = (item.Tag as GCFestivalBase).Text;
                dlg.OnButtonYes += new TBButtonPressed(onDeleteEvent);
                dlgCtrl.ShowInContainer(Controller.ViewContainer, GVControlAlign.Fill);
            }
        }

        /// <summary>
        /// Export events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text file (*.txt)|*.txt|XML File (*.xml)|*.xml||";
            sfd.FilterIndex = 0;
            sfd.CheckPathExists = true;
            sfd.OverwritePrompt = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                GCFestivalBookCollection.Export(sfd.FileName, sfd.FilterIndex);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (changingFilter)
                return;
            if (textBox1.Text.Length == 0)
            {
                FillListView();
                return;
            }

            changingFilter = true;
            string[] sp = textBox1.Text.Trim().ToLower().Split(' ');
            int A, B;

            updatingList = true;
            listBox1.BeginUpdate();
            listBox1.Items.Clear();

            foreach (GCFestivalBook book in GCFestivalBookCollection.Books)
            {
                foreach (GCFestivalBase fb in book.Festivals)
                {
                    A = 0;
                    B = 0;
                    if (sp.Length > 0)
                    {
                        foreach (string s in sp)
                        {
                            A++;
                            if (fb.Text.IndexOf(s, 0, StringComparison.CurrentCultureIgnoreCase) >= 0)
                            {
                                B++;
                            }
                        }
                    }

                    if (A == B)
                    {
                        GCListBoxEntry lb = new GCListBoxEntry();
                        fb.updateListBoxEntry(lb);
                        listBox1.Items.Add(lb);
                    }

                }
            }
            updatingList = false;
            listBox1.EndUpdate();
            changingFilter = false;
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

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= listBox1.Items.Count)
                return;
            if (listBox1.Items[e.Index] is GCListBoxEntry)
            {
                GCListBoxEntry lb = listBox1.Items[e.Index] as GCListBoxEntry;
                if ((e.State & DrawItemState.Selected) != 0)
                {
                    e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GCListBoxEntry item = GetSelectedItem();
            if (item != null && item.Tag != null)
            {
                if (item.Tag is GCFestivalTithiMasa)
                {
                }
                else if (item.Tag is GCFestivalEkadasi)
                {
                }
                else if (item.Tag is GCFestivalMasaDay)
                {
                }
                else if (item.Tag is GCFestivalRelated)
                {
                }
                else if (item.Tag is GCFestivalSankranti)
                {
                }
                else if (item.Tag is GCFestivalSpecial)
                {
                }
            }
        }


        public void SetCollectionId(int collId)
        {
            int i, c = 0;
            GCFestivalBook newFilterToBook = null;
            listBoxFestivalBooks.Items.Clear();
            listBoxFestivalBooks.Items.Add("All Collections");
            for (i = 0; i < GCFestivalBookCollection.Books.Count; i++)
            {
                GCFestivalBook fb = GCFestivalBookCollection.Books[i];
                if (fb.CollectionId == collId)
                {
                    newFilterToBook = fb;
                    c = i;
                }
                listBoxFestivalBooks.Items.Add(fb);
            }

            listBoxFestivalBooks.SelectedIndex = c;

        }

        //private Font listBoxFestivalBooks_Font = new Font("Helvetica", 10f);
        //private StringFormat listBoxFestivalBooks_Format = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };

        private void listBoxFestivalBooks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (changingFilter)
                return;
            changingFilter = true;
            textBox1.Text = "";
            int si = listBoxFestivalBooks.SelectedIndex;
            if (listBoxFestivalBooks.Items.Count > si && si >= 0)
            {
                if (listBoxFestivalBooks.Items[si] is GCFestivalBook)
                    filterToBook = (GCFestivalBook)listBoxFestivalBooks.Items[si];
                else
                    filterToBook = null;
            }

            FillListView();
            changingFilter = false;

        }

        /*private void listBoxFestivalBooks_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= listBoxFestivalBooks.Items.Count)
                return;

            object obj = listBoxFestivalBooks.Items[e.Index];
            string text = "";
            if (obj is string)
            {
                text = (string)obj;
            }
            else if (obj is GCFestivalBook)
            {
                text = ((GCFestivalBook)obj).CollectionName;
            }

            SizeF sf = e.Graphics.MeasureString(text, listBoxFestivalBooks_Font, listBoxFestivalBooks.Width - 20);
            e.ItemHeight = (int)sf.Height + 20;
        }

        private void listBoxFestivalBooks_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= listBoxFestivalBooks.Items.Count)
                return;

            object obj = listBoxFestivalBooks.Items[e.Index];
            string text = "";
            if (obj is string)
            {
                text = (string)obj;
            }
            else if (obj is GCFestivalBook)
            {
                text = ((GCFestivalBook)obj).CollectionName;
            }

            if ((e.State & DrawItemState.Selected) != 0)
                e.Graphics.FillRectangle(Brushes.LightYellow, e.Bounds);
            else
                e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
            Rectangle rect = e.Bounds;
            rect.Inflate(-10, -10);
            e.Graphics.DrawString(text, listBoxFestivalBooks_Font, Brushes.Black, rect, listBoxFestivalBooks_Format);

        }*/

        private void button5_Click(object sender, EventArgs e)
        {
            Controller.Parent.ExecuteMessage("setTabChanged", new GSString("festivalBooks"));
        }
    }


    public class EventsPanelDelegate : GVCore
    {
        public EventsPanelDelegate(EventsPanel v)
        {
            View = v;
            v.Controller = this;
        }

        public override GSCore ExecuteMessage(string token, GSCoreCollection args)
        {
            if (token.Equals("setCollection"))
            {
                int num = (int)args.getSafe(0).getIntegerValue();

                EventsPanel ep = (EventsPanel)View;
                ep.SetCollectionId(num);
                return GSCore.Void;
            }
            else
            {
                return base.ExecuteMessage(token, args);
            }
        }
    }


}



















