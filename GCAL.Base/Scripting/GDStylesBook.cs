using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCAL.Base.Scripting
{
    public class GDStylesBook: IEnumerable
    {
        private List<GDStyleDefinition> p_list = null;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return p_list.GetEnumerator();
        }

        public GDStyleDefinition AddStyle(GDStyleDefinition node)
        {
            SafeList();

            p_list.Add(node);

            return node;
        }

        public void Remove(GDStyleDefinition node)
        {
            if (p_list == null)
                return;

            p_list.Remove(node);
        }

        public void RemoveStyle(string styleName)
        {
            for (int i = 0; i < Count; i++)
            {
                if (p_list[i].Name.Equals(styleName))
                {
                    p_list.RemoveAt(i);
                    i--;
                }
            }
        }

        public void RemoveAt(int index)
        {
            if (p_list == null)
                return;

            p_list.RemoveAt(index);
        }

        public GDStyleDefinition this[string styleName]
        {
            get
            {
                SafeList();
                foreach (GDStyleDefinition sd in p_list)
                {
                    if (sd.Name.Equals(styleName))
                        return sd;
                }
                GDStyleDefinition sdn = new GDStyleDefinition();
                sdn.Name = styleName;
                AddStyle(sdn);
                return sdn;
            }
        }

        public GDStyleDefinition this[int index]
        {
            get
            {
                if (p_list == null)
                    return null;
                return p_list[index];
            }
            set
            {
                SafeList();
                p_list[index] = value;
            }
        }

        public int Count
        {
            get
            {
                if (p_list == null)
                    return 0;
                return p_list.Count;
            }
        }

        private void SafeList()
        {
            if (p_list == null)
            {
                p_list = new List<GDStyleDefinition>();
            }
        }
    }
}
