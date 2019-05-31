using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.CalendarDataView
{
    public interface CDVDataSource
    {
        void AsyncRequestData(CDVDataTarget requestor, CDVDocumentCell data);
        void SyncRefreshLayout(CDVDataTarget requestor, CDVDocumentCell data);
    }

    public interface CDVDataTarget
    {
        void OnCDVDataAvailable(CDVDocumentCell data);
        CDVAtom GetDocument();
    }
}
