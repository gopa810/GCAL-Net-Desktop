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
    public class MasaListTabController: GVCore
    {
        public MasaListTabController(MasaListTab v)
        {
            View = v;
            v.Controller = this;
        }

        public MasaListTab getView()
        {
            return View as MasaListTab;
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

            return GSCore.Void;
        }

    }
}
