using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.MapCalculator
{
    public enum QuadrantResultState
    {
        NotAvailable = -1,
        Consistent = 0,
        Inconsistent = 1,
        Decomposable = 2
    }
}
