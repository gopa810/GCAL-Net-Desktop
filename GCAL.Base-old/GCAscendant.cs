using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base
{
    public class GCAscendant
    {
        public GCEarthData Earth { get; set; }

        public GregorianDateTime CurrentDateTime { get; set; }

        public int CurrentSign {get;set;}

        public void GetNextAscendent()
        {
            GregorianDateTime nd;
            CurrentSign = Earth.GetNextAscendentStart(CurrentDateTime, out nd);
            CurrentDateTime = nd;
        }

        public bool GetNextAscendantBefore(GregorianDateTime stopDateTime)
        {
            if (stopDateTime.GetJulianComplete() < CurrentDateTime.GetJulianComplete())
                return false;
            GetNextAscendent();
            return (stopDateTime.GetJulianComplete() > CurrentDateTime.GetJulianComplete());
        }

        public string CurrentSignName
        {
            get
            {
                return GCRasi.GetName(CurrentSign);
            }
        }
    }
}
