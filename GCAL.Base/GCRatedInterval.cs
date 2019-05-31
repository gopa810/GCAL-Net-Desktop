using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GCAL.Base.Scripting;

namespace GCAL.Base
{
    public class GCRatedInterval: GSCore
    {
        public GregorianDateTime startTime;
        public GregorianDateTime endTime;
        public double ratingPos = 0.0;
        public double ratingNeg = 0.0;
        public string Title;
        public string[] Notes;
        public int ResultRatio = 0;

        public class IntervalComparer : Comparer<GCRatedInterval>
        {
            public override int Compare(GCRatedInterval x, GCRatedInterval y)
            {
                double A, B;
                A = x.ratingPos + x.ratingNeg;
                B = y.ratingPos + y.ratingNeg;

                if (x.ResultRatio < y.ResultRatio) return 1;
                else if (x.ResultRatio > y.ResultRatio) return -1;

                int dc = x.startTime.CompareYMD(y.startTime);
                if (dc != 0) return dc;

                if (A < B) return 1;
                else if (A > B) return -1;

                if (x.ratingPos < y.ratingPos) return 1;
                else if (x.ratingPos > y.ratingPos) return -1;

                return 0;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} - {2} {3} :: {4} [{5}]", startTime.ToString(), startTime.LongTimeString(), endTime.ToString(), 
                endTime.LongTimeString(), Title, ratingPos + ratingNeg);
        }

        public double Length
        {
            get
            {
                if (startTime == null || endTime == null)
                    return -1.0;
                return endTime.GetJulianComplete() - startTime.GetJulianComplete();
            }
        }

        public override GSCore GetPropertyValue(string s)
        {
            switch (s)
            {
                case "startTime":
                    return startTime;
                case "endTime":
                    return endTime;
                case "ratingPos":
                    return new GSNumber(ratingPos);
                case "ratingNeg":
                    return new GSNumber(ratingNeg);
                case "ratingTotal":
                    return new GSNumber(ratingPos + ratingNeg);
                case "length":
                    return new GSNumber(Length);
                case "title":
                    return new GSString(Title);
                case "notes":
                    {
                        GSList list = new GSList();
                        foreach(string str in Notes)
                            list.Parts.Add(new GSString(str));
                        return list;
                    }
                case "ratio":
                    return new GSNumber(ResultRatio);
                default:
                    return base.GetPropertyValue(s);
            }
        }

    }
}
