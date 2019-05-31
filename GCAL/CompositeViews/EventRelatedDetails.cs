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
    public partial class EventRelatedDetails : UserControl
    {
        public EventRelatedDetailsController Controller { get; set; }
        public GCFestivalRelated p_related { get; set; }
        public GCFestivalBase p_eventObject;

        public event TBButtonPressed OnEventDetailChanged;

        public EventRelatedDetails()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Controller.RemoveFromContainer();
        }

        public void setEvent(GCFestivalRelated r)
        {
            if (r == null)
            {
                editEventBase2.EventTitle = "(Untitled)";
                editEventBase2.EventStartYear = -10000;
                editEventBase2.EventFastingSubject = string.Empty;
                editEventBase2.EventBook = -1;
                editEventBase2.DaysOffset = 0;
                editEventBase2.Fast = FastType.FAST_NULL;
            }
            else
            {
                editEventBase2.EventTitle = r.Text;
                editEventBase2.EventStartYear = r.StartYear;
                editEventBase2.EventFastingSubject = r.FastingSubject;
                editEventBase2.EventBook = r.BookID;
                editEventBase2.DaysOffset = r.DayOffset;
                editEventBase2.Fast = r.FastID;
            }

            p_related = r;
        }

        public void setParentEvent(GCFestivalBase fb)
        {
            p_eventObject = fb;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            bool isNewEvent = false;
            if (p_related == null)
            {
                p_related = new GCFestivalRelated();
                isNewEvent = true;
            }

            if (p_related != null)
            {
                GCFestivalBook book = null;
                if (p_eventObject.BookID >= 0)
                    book = GCFestivalBookCollection.getSafeBook(p_eventObject.BookID);

                if (p_related.BookID != editEventBase2.EventBook)
                {
                    p_related.BookID = editEventBase2.EventBook;
                    if (book != null) book.Changed = true;
                }
                if (p_related.DayOffset != editEventBase2.DaysOffset)
                {
                    p_related.DayOffset = editEventBase2.DaysOffset;
                    if (book != null) book.Changed = true;
                }
                if (p_related.FastID != editEventBase2.Fast)
                {
                    p_related.FastID = editEventBase2.Fast;
                    if (book != null) book.Changed = true;
                }
                if (p_related.FastingSubject == null ||
                    !p_related.FastingSubject.Equals(editEventBase2.EventFastingSubject))
                {
                    p_related.FastingSubject = editEventBase2.EventFastingSubject;
                    if (book != null) book.Changed = true;
                }
                if (p_related.StartYear != editEventBase2.EventStartYear)
                {
                    p_related.StartYear = editEventBase2.EventStartYear;
                    if (book != null) book.Changed = true;
                }
                if (p_related.Text == null ||
                    !p_related.Text.Equals(editEventBase2.EventTitle))
                {
                    p_related.Text = editEventBase2.EventTitle;
                    if (book != null) book.Changed = true;
                }

                if (isNewEvent)
                {
                    p_eventObject.AddRelatedFestival(p_related);
                }

                p_related = null;

                if (OnEventDetailChanged != null)
                    OnEventDetailChanged(this, e);
            }

            Controller.RemoveFromContainer();
        }

    }

    public class EventRelatedDetailsController : GVCore
    {
        public EventRelatedDetailsController(EventRelatedDetails v)
        {
            View = v;
            v.Controller = this;
        }
    }
}
