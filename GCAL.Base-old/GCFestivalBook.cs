using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base
{
    public class GCFestivalBook
    {
        private List<GCFestivalBase> list = new List<GCFestivalBase>();
        public int CollectionId { get; set; }
        public string CollectionName { get; set; }
        public bool Visible { get; set; }
        public string FileName { get; set; }
        public bool Changed { get; set; }


        public GCFestivalBook()
        {
            CollectionId = 8;
            CollectionName = string.Empty;
            Visible = true;
            Changed = false;
        }

        public override string ToString()
        {
            return CollectionName;
        }

        public List<GCFestivalBase> Festivals
        {
            get
            {
                return list;
            }
        }


        public int Count()
        {
            return list.Count;
        }

        /* OLD STYLE FASTING
         * 
        public static int SetOldStyleFasting(int bOldStyle)
        {
            GCFestivalTithiMasa pce = null;

            int[,] locMatrix =
	        {
		        // visnu tattva and sakti tattva
         * // tithi, masa, fast, new fast, class
		        { 28,   0,  4,  7,  0},
		        { 29,   3,  1, 7, 0},
		        { 22,   4,  1, 0, 0},
		        { 26,   4,  1, 7, 0},
		        { 21,   9,  1, 7, 0},
		        { 26,   9,  1, 7, 0},
		        { 27,   9,  1, 7, 0},
		        {  7,   4,  5, 7, 0},
		        { 29,  10,  3, 7, 0},
		        { 23,  11,  2, 7, 0},
		        // acaryas
		        {  8,   4,  1, 0, 2},
		        { 27,   4,  1, 0, 2},
		        { 14,   2,  1, 0, 2},
		        { 25,   6,  1, 0, 2},
		        { 18,   6,  1, 0, 2},
		        {  3,   8,  1, 0, 2},
		        {  4,  10,  1, 0, 2},
		        {-1,-1,-1,-1, -1}
	        };

            int i, idx;
            int ret = 0;
            if (bOldStyle != 0) idx = 2;
            else idx = 3;

            for (i = 0; locMatrix[i, 0] >= 0; i++)
            {
                foreach (GCFestivalBook book in GCFestivalBookCollection.Books)
                {
                    foreach (GCFestivalBase fb in book.Festivals)
                    {
                        if (fb is GCFestivalTithiMasa)
                        {
                            pce = fb as GCFestivalTithiMasa;
                            if (pce.nMasa == locMatrix[i, 1] &&
                                pce.nTithi == locMatrix[i, 0] &&
                                pce.nClass == locMatrix[i, 4])
                            {
                                if (pce.nFastType != locMatrix[i, idx])
                                {
                                    ret++;
                                    pce.nFastType = locMatrix[i, idx];
                                }
                                break;
                            }
                        }
                    }
                }
            }

            return ret;
        }
        */


        public GCFestivalBase Add(GCFestivalBase ce)
        {
            list.Add(ce);
            return ce;
        }

        public void Clear()
        {
            list.Clear();
        }

        public void Remove(GCFestivalBase ce)
        {
            int index = -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].EventID == ce.EventID)
                {
                    index = i;
                    break;
                }
            }
            if (index >= 0)
            {
                Changed = true;
                list.RemoveAt(index);
            }
        }
    }
}
