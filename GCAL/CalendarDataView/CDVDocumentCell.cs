using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.CalendarDataView
{
    public class CDVDocumentCell
    {
        public string Key = "";
        public string PrevKey = "";
        public string NextKey = "";
        public CDVAtom Item = null;
        public bool measured = false;
        public object UserData = null;
        public bool RefreshLayout = false;

        public void MoveAfter(CDVDocumentCell after)
        {
            Item.Offset(0, after.Item.Bounds.Bottom - Item.Bounds.Location.Y);
        }

        public void MoveBefore(CDVDocumentCell before)
        {
            Item.Offset(0, before.Item.Bounds.Top - Item.Size.Height - Item.Bounds.Location.Y);
        }

        public void Prepare(CDVContext ctx, int clientWidth)
        {
            if (!measured)
            {
                ctx.rulersReflow = false;
                Item.MeasureRect(ctx, clientWidth);
                if (ctx.rulersChanged)
                {
                    ctx.rulersReflow = true;
                    Item.MeasureRect(ctx, clientWidth);
                }
                Item.ApplySpanWidths(clientWidth);
                Item.ApplyContentAlignment();
                measured = true;
            }
        }
    }
}
