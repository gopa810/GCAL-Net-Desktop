using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCAL.Base;
using GCAL.Views;
using GCAL.Base.Scripting;

namespace GCAL.CompositeViews
{
    public class SearchTabController: GVCore
    {
        public SearchTabController(SearchTab v)
        {
            View = v;
            v.Controller = this;
        }

    }
}
