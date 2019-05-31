using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCAL.Base.Scripting;
using GCAL.Base;
using GCAL.Views;

namespace GCAL.CompositeViews
{
    public class CoreEventsTabController: GVCore
    {
        public CoreEventsTabController(CoreEventsTab v)
        {
            v.Controller = this;
            View = v;
        }

        public CoreEventsTab getView()
        {
            return View as CoreEventsTab;
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
