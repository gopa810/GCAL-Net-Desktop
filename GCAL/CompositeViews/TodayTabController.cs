using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using GCAL.Base;
using GCAL.Base.Scripting;
using GCAL.Views;
using GCAL.CompositeViews;

namespace GCAL.CompositeViews
{
    public class TodayTabController: GVCore
    {
        public TodayTabController(TodayTab v)
        {
            View = v;
            v.Controller = this;
        }

        public TodayTab getView()
        {
            return View as TodayTab;
        }

        public override GSCore ExecuteMessage(string token, GSCoreCollection args)
        {
            switch (token)
            {
                case "getCurrentContent":
                    return getView().getCurrentContent();
                default:
                    return base.ExecuteMessage(token, args);
            }
        }

    }
}
