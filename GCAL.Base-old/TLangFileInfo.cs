using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base
{
    public class TLangFileInfo
    {
        public string m_strAcr;
        public string m_strLang;
        public string m_strFile;

        public void decode(string p)
        {
            m_strAcr = "";
            m_strLang = "";
            m_strFile = "";
            if (p == null) return;
            string[] ps = p.Split('#');
            if (ps.Length == 3)
            {
                m_strAcr = ps[0];
                m_strLang = ps[1];
                m_strFile = ps[2];
            }
        }
        public string encode()
        {
            return string.Format("{0}#{1}#{2}", m_strAcr, m_strLang, m_strFile);
        }

    }
}
