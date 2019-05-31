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
    public partial class EventDetails : UserControl
    {
        public EventDetailsController Controller { get; set; }

        public event TBButtonPressed OnButtonOK;
        public event TBButtonPressed OnButtonCancel;
        public event TBButtonPressed OnButtonRelated;

        private GCFestivalBase p_eventObject = null;
        EditEventTithiMasa panelTM = null;
        EditEventMasaDay panelMD = null;
        EditEventSankranti panelS = null;
        EditEventEkadasi panelE = null;
        EditEventRelated panelR = null;
        EditEventSpecial panelSP = null;

        public EventDetails()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (EventObject != null)
            {
                if (EventObject.BookID != editEventBase1.EventBook)
                {
                    if (EventObject.BookID != -1)
                    {
                        GCFestivalBook oldBook = GCFestivalBookCollection.getSafeBook(EventObject.BookID);
                        oldBook.Remove(EventObject);
                        oldBook.Changed = true;
                    }

                    GCFestivalBook newBook = GCFestivalBookCollection.getSafeBook(editEventBase1.EventBook);
                    EventObject.BookID = editEventBase1.EventBook;
                    newBook.Add(EventObject);
                    newBook.Changed = true;
                }

                GCFestivalBook eventBook = GCFestivalBookCollection.getSafeBook(EventObject.BookID);

                if (EventObject.DayOffset != editEventBase1.DaysOffset)
                {
                    EventObject.DayOffset = editEventBase1.DaysOffset;
                    eventBook.Changed = true;
                }
                if (EventObject.Text != editEventBase1.EventTitle)
                {
                    EventObject.Text = editEventBase1.EventTitle;
                    eventBook.Changed = true;
                }
                if (EventObject.FastingSubject != editEventBase1.EventFastingSubject)
                {
                    EventObject.FastingSubject = editEventBase1.EventFastingSubject;
                    eventBook.Changed = true;
                }
                if (EventObject.StartYear != editEventBase1.EventStartYear)
                {
                    EventObject.StartYear = editEventBase1.EventStartYear;
                    eventBook.Changed = true;
                }
                if (EventObject.FastID != editEventBase1.Fast)
                {
                    EventObject.FastID = editEventBase1.Fast;
                    eventBook.Changed = true;
                }

                if (panelTM != null && EventObject is GCFestivalTithiMasa)
                {
                    if (panelTM.updateEvent(EventObject as GCFestivalTithiMasa))
                        eventBook.Changed = true;
                }
                if (panelMD != null && EventObject is GCFestivalMasaDay)
                {
                    if (panelMD.updateEvent(EventObject as GCFestivalMasaDay))
                        eventBook.Changed = true;
                }
                if (panelS != null && EventObject is GCFestivalSankranti)
                {
                    if (panelS.updateEvent(EventObject as GCFestivalSankranti))
                        eventBook.Changed = true;
                }
                if (panelE != null && EventObject is GCFestivalEkadasi)
                {
                    if (panelE.updateEvent(EventObject as GCFestivalEkadasi))
                        eventBook.Changed = true;
                }
            }

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

        private void buttonRelatedEvents_Click(object sender, EventArgs e)
        {
            EventRelatedList d = new EventRelatedList();
            EventRelatedListController dc = new EventRelatedListController(d);
            d.ViewContainer = Controller.ViewContainer;
            d.EventObject = EventObject;
            d.FillRelativeEvents();

            if (Controller.ViewContainer != null)
                Controller.ViewContainer.AddControl(dc, GVControlAlign.Fill);

            if (OnButtonRelated != null)
                OnButtonRelated(this, e);
        }


        public GCFestivalBase EventObject
        {
            get
            {
                return p_eventObject;
            }
            set
            {
                p_eventObject = value;

                editEventBase1.DaysOffset = value.DayOffset;
                editEventBase1.EventTitle = value.Text;
                editEventBase1.EventFastingSubject = value.FastingSubject;
                editEventBase1.EventStartYear = value.StartYear;
                editEventBase1.Fast = value.FastID;
                editEventBase1.EventBook = value.BookID;
                Control newControl = null;

                if (value is GCFestivalTithiMasa)
                {
                    panelTM = new EditEventTithiMasa();
                    panelTM.setEvent(value as GCFestivalTithiMasa);
                    newControl = panelTM;
                }
                else if (value is GCFestivalEkadasi)
                {
                    panelE = new EditEventEkadasi();
                    panelE.setEvent(value as GCFestivalEkadasi);
                    newControl = panelE;
                }
                else if (value is GCFestivalMasaDay)
                {
                    panelMD = new EditEventMasaDay();
                    panelMD.setEvent(value as GCFestivalMasaDay);
                    newControl = panelMD;
                }
                else if (value is GCFestivalSankranti)
                {
                    panelS = new EditEventSankranti();
                    panelS.setEvent(value as GCFestivalSankranti);
                    newControl = panelS;
                }
                else if (value is GCFestivalSpecial)
                {
                    panelSP = new EditEventSpecial();
                    panelSP.setEvent(value as GCFestivalSpecial);
                    newControl = panelSP;
                }

                if (newControl != null)
                {
                    newControl.Parent = panel2;
                    panel2.Controls.Add(newControl);
                    panel2.Size = new Size(panel2.Size.Width, newControl.Size.Height);
                    newControl.Dock = DockStyle.Fill;
                }
            }
        }

    }

    public class EventDetailsController : GVCore
    {
        public EventDetailsController(EventDetails v)
        {
            View = v;
            v.Controller = this;
        }
    }
}
