using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCAL.Base;
using GCAL.Base.Scripting;
using GCAL.Views;

namespace GCAL.CompositeViews
{
    public class CalendarTabController: GVCore
    {
        public CalendarTabController(CalendarTab v)
        {
            v.Controller = this;
            View = v;
        }

        public CalendarTab getView()
        {
            return View as CalendarTab;
        }


        public override GSCore ExecuteMessage(string token, GSCoreCollection args)
        {
            switch (token)
            {
                case "getEndDone":
                    {
                        GSCore gtObj = args.getSafe(0);
                        if (gtObj != null && gtObj is GregorianDateTime)
                        {
                            GregorianDateTime gdt = gtObj as GregorianDateTime;
                            if (gdt.GetJulianInteger() < getView().calStartDate.GetJulianInteger() + 7)
                            {
                                gdt = new GregorianDateTime(getView().calStartDate);
                                gdt.AddDays(7);
                            }
                            getView().calEndDate = gdt;
                            //getView().EndDateText(gdt.ToString());
                            getView().Recalculate();
                        }
                    }
                    break;
                case "getCurrentContent":
                    return getView().getCurrentContent();
                default:
                    return base.ExecuteMessage(token, args);
            }

            return GSCore.Void;
        }
    }
}
